using System;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using System.Reflection;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RainbowMage.OverlayPlugin.Updater
{
    public static class HttpClientWrapper
    {
        public delegate bool ProgressInfoCallback(long resumed, long dltotal, long dlnow, long ultotal, long ulnow);

        public static void Init(string pluginDirectory)
        {
            // Nothing to do here
        }

        public static string Get(string url)
        {
            return Get(url, new Dictionary<string, string>(), null, null, false);
        }

        public static string Get(string url, Dictionary<string, string> headers, string downloadDest,
            ProgressInfoCallback infoCb, bool resume)
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "NuGet Client V3/3.4.2");
            client.Headers.Add("X-NuGet-Client-Version", "3.4.2");
            
            foreach (var key in headers.Keys)
            {
                client.Headers.Add(key, headers[key]);
            }

            var completionLock = new object();
            string result = null;
            Exception error = null;
            var retry = false;

            Action action = (async () => {
                try {
                    if (downloadDest == null)
                    {
                        result = await client.DownloadStringTaskAsync(url);
                    }
                    else
                    {
                        var tcs = new TaskCompletionSource<object>(url);

                        AsyncCompletedEventHandler completedHandler = (cs, ce) =>
                        {
                            if (ce.UserState == tcs)
                            {
                                if (ce.Error != null)
                                {
                                    tcs.TrySetException(ce.Error);
                                }
                                else if (ce.Cancelled) tcs.TrySetCanceled();
                                else tcs.TrySetResult(null);
                            }
                        };

                        DownloadProgressChangedEventHandler progressChangedHandler = (ps, pe) =>
                        {
                            if (pe.UserState == tcs || infoCb != null)
                                infoCb(0, pe.TotalBytesToReceive, pe.BytesReceived, 0, 0);
                        };

                        client.DownloadFileCompleted += completedHandler;
                        client.DownloadProgressChanged += progressChangedHandler;

                        try
                        {
                            client.DownloadFileAsync(new Uri(url), downloadDest, tcs);

                            await tcs.Task;
                        }
                        finally
                        {
                            client.DownloadFileCompleted -= completedHandler;
                            client.DownloadProgressChanged -= progressChangedHandler;
                        }
                    }
                } catch (IOException e)
                {
                    error = e;
                    retry = true;
                } catch(UnauthorizedAccessException e)
                {
                    error = e;
                    retry = true;
                } catch (HttpRequestException e)
                {
                    error = e;
                    retry = true;
                } catch (Exception e)
                {
                    error = e;
                }

                lock (completionLock)
                {
                    Monitor.Pulse(completionLock);
                }
            });
            action();

            lock (completionLock)
            {
                Monitor.Wait(completionLock);
            }

            if (error != null)
            {
                throw new CurlException(retry, error.Message, error);
            }
            return result;
        }
    }
}
