namespace ConfigurationStoreBackup
{
    internal class KeyLabel
    {
        public string Key { get; }
        public string Label { get; }

        public KeyLabel(string key, string label)
        {
            Key = key;
            Label = label;
        }
    }
}
