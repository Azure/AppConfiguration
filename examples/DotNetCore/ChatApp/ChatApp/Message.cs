using Microsoft.Extensions.Configuration;

namespace ChatApp
{
    internal class Message
    {
        [ConfigurationKeyName("role")]
        public string Role { get; set; }

        [ConfigurationKeyName("content")]
        public string Content { get; set; }
    }
}
