using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib
{
    public class Mapping
    {
        public Mapping()
        {
        }

        public Mapping(string inBaseName, List<string> inBaseValues, string inOtherName, List<string> inOtherValues)
        {
            _baseName = inBaseName;
            _baseValues = inBaseValues;

            _otherName = inOtherName;
            _otherValues = inOtherValues;
        }

        string _separator = ".";
        public string Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        string _baseName = string.Empty;
        public string BaseName
        {
            get { return _baseName; }
            set { _baseName = value; }
        }

        string _otherName = string.Empty;
        public string OtherName
        {
            get { return _otherName; }
            set { _otherName = value; }
        }

        List<string> _baseValues = new List<string>();
        public List<string> BaseValues
        {
            get { return _baseValues; }
            set { _baseValues = value; }
        }

        List<string> _otherValues = new List<string>();
        public List<string> OtherValues
        {
            get { return _otherValues; }
            set { _otherValues = value; }
        }

        SortedDictionary<string, string> _mapping = new SortedDictionary<string, string>();
        public string SerializedMapping
        {
            get { return SerializationHelper.SerializeDictionary(_mapping); }
            set { _mapping = SerializationHelper.DeserializeSortedDictionary(value); }
        }

        private SortedDictionary<string, string> GetReversedDic()
        {
            SortedDictionary<string, string> reversedMapping = new SortedDictionary<string, string>();

            foreach(string key in _mapping.Keys)
            {
                reversedMapping.Add(_mapping[key], key);
            }

            return reversedMapping;
        }

        public SortedDictionary<string, string> GetMapping(string inObjectName)
        {
            if (inObjectName == _baseName)
            {
                return _mapping;
            }

            return GetReversedDic();
        }

        public void AutoMap()
        {
            _mapping.Clear();

            List<string> remainingValues = new List<string>(_otherValues);

            foreach (string key in _baseValues)
            {
                string candidate = string.Empty;
                if (remainingValues.Contains(key))
                {
                    candidate = key;
                }
                else
                {
                    string replaced = key.Replace(_baseName, _otherName);
                    if (remainingValues.Contains(replaced))
                    {
                        candidate = replaced;
                    }
                    else
                    {
                        string[] split = key.Split(_separator.ToCharArray());
                        replaced = split[split.Length - 1];

                        foreach (string value in remainingValues)
                        {
                            string[] otherSplit = value.Split(_separator.ToCharArray());
                            string shortened = otherSplit[otherSplit.Length - 1];

                            if (shortened == replaced)
                            {
                                candidate = value;
                                break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(candidate))
                {
                    _mapping.Add(key, candidate);
                    remainingValues.Remove(candidate);
                }
            }
        }

        public bool IsComplete()
        {
            return _baseValues.Count == _otherValues.Count && _otherValues.Count == _mapping.Count;
        }

        public List<string> GetBaseOrphans()
        {
            List<string> orphans = new List<string>();

            if (!IsComplete())
            {
                foreach (string baseValue in _baseValues)
                {
                    if (!_mapping.ContainsKey(baseValue))
                    {
                        orphans.Add(baseValue);
                    }
                }
            }

            return orphans;
        }

        public List<string> GetOtherOrphans()
        {
            List<string> orphans = new List<string>();

            if (!IsComplete())
            {
                foreach (string otherValue in _otherValues)
                {
                    if (!_mapping.ContainsValue(otherValue))
                    {
                        orphans.Add(otherValue);
                    }
                }
            }

            return orphans;
        }

        public SortableBindingList<MappingItem> ToItems()
        {
            SortableBindingList<MappingItem> items = new SortableBindingList<MappingItem>();
            PossibleMappingsConverter.values = _baseValues;
            PossibleOtherMappingsConverter.values = _otherValues;

            foreach(string key in _mapping.Keys)
            {
                items.Add(new MappingItem(this, key, _mapping[key]));
            }

            List<string> orphans = GetBaseOrphans();
            foreach (string key in orphans)
            {
                items.Add(new MappingItem(this, key, string.Empty));
            }

            orphans = GetOtherOrphans();
            foreach (string key in orphans)
            {
                items.Add(new MappingItem(this, string.Empty, key));
            }

            return items;
        }
    }
}
