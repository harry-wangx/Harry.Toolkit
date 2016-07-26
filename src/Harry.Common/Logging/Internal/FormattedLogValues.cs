// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Harry.Logging.Internal
{
    /// <summary>
    /// LogValues to enable formatting options supported by <see cref="M:string.Format"/>. 
    /// This also enables using {NamedformatItem} in the format string.
    /// </summary>
    public class FormattedLogValues : IList<KeyValuePair<string, object>>
    {

        readonly object locker=new object();
        private static Dictionary<string, LogValuesFormatter> _formatters = new Dictionary<string, LogValuesFormatter>();

        private readonly LogValuesFormatter _formatter;
        private readonly object[] _values;
        private readonly string _originalMessage;

        public FormattedLogValues(string format, params object[] values)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (values.Length != 0)
            {
                if (_formatters.ContainsKey(format))
                {
                    _formatter = _formatters[format];
                }
                else
                {
                    lock (locker)
                    {
                        if (_formatters.ContainsKey(format))
                        {
                            _formatter = _formatters[format];
                        }
                        else
                        {
                            _formatters.Add(format, new LogValuesFormatter(format));
                            _formatter = _formatters[format];
                        }
                    }
                }
            }

            _originalMessage = format;
            _values = values;
        }

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                if (index == Count - 1)
                {
                    return new KeyValuePair<string, object>("{OriginalFormat}", _originalMessage);
                }

                return _formatter.GetValue(_values, index);
            }
            set
            {
                throw new NotImplementedException();
            }

        }

        public int Count
        {
            get
            {
                if (_formatter == null)
                {
                    return 1;
                }

                return _formatter.ValueNames.Count + 1;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        public override string ToString()
        {
            if (_formatter == null)
            {
                return _originalMessage;
            }

            return _formatter.Format(_values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }
    }
}

