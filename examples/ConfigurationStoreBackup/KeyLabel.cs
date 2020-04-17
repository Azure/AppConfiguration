using System;

namespace ConfigurationStoreBackup
{
    internal struct KeyLabel
    {
        internal string Key { get; }
        internal string Label { get; }

        public KeyLabel(string key, string label)
        {
            Key = key;
            Label = label;
        }
        
        public override bool Equals(Object obj)
        {
            if (obj is KeyLabel)
            {
                KeyLabel keyLabel = (KeyLabel)obj;
                return string.Equals(this.Key, keyLabel.Key, StringComparison.Ordinal)
                    && string.Equals(this.Label, keyLabel.Label, StringComparison.Ordinal);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Label != null ? this.Key.GetHashCode() ^ this.Label.GetHashCode() : this.Key.GetHashCode();
        }
    }
}
