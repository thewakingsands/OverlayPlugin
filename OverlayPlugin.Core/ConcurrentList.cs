using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RainbowMage.OverlayPlugin
{
    public sealed class ConcurrentList<T> : IList<T>
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly IList<T> _list;
        public ConcurrentList()
        {
            _list = new List<T>();
        }
        public ConcurrentList(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            else
            {
                _list = new List<T>(list);
            }

        }

        T IList<T>.this[int index]
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _list[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }

            }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _list[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        int ICollection<T>.Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _list.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }

            }
        }

        bool ICollection<T>.IsReadOnly => _list.IsReadOnly;

        void ICollection<T>.Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        void ICollection<T>.Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }

        bool ICollection<T>.Contains(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterReadLock();
            try
            {
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new List<T>(_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<T>(_list).GetEnumerator();
        }

        int IList<T>.IndexOf(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        void IList<T>.RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        ~ConcurrentList()
        {
            if (_lock != null)
            {
                _lock.Dispose();
            }
        }
    }
}
