using System;
using System.Collections.Generic;

namespace ConfigurationStoreBackup
{
    internal class KeyLabelComparer : IEqualityComparer<KeyLabel>
    {
        public bool Equals(KeyLabel x, KeyLabel y)
        {
            return string.Equals(x.Key, y.Key, StringComparison.Ordinal)
                && string.Equals(x.Label, y.Label, StringComparison.Ordinal);
        }

        public int GetHashCode(KeyLabel obj)
        {
            return obj.Label != null ? obj.Key.GetHashCode() ^ obj.Label.GetHashCode() : obj.Key.GetHashCode();
        }
    }
}
