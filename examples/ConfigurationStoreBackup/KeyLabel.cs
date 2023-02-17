using System;

namespace ConfigurationStoreBackup
{
    internal struct KeyLabel
    {
        public string Key { get; }
        public string Label { get; }

        public KeyLabel(string key, string label)
        {
            Key = key;
            Label = label ?? string.Empty;
        }

        public override int GetHashCode()
        {
            return this.Label != null ? this.Key.GetHashCode() ^ this.Label.GetHashCode() : this.Key.GetHashCode();
        }
    }
}
