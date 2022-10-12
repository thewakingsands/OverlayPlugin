using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin.Controls
{
    public partial class LogPanel : UserControl
    {
        public LogPanel(TinyIoCContainer container)
        {
            InitializeComponent();

            container.Resolve<ILogger>().RegisterListener((entry) =>
            {
                logBox.AppendText($"[{entry.Time}] {entry.Level}: {entry.Message}" + Environment.NewLine);
            });
        }
    }
}
