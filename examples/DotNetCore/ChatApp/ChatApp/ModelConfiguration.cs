using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace ChatApp
{
    internal class ModelConfiguration
    {
        [ConfigurationKeyName("model")]
        public string Model { get; set; }

        [ConfigurationKeyName("messages")]
        public List<Message> Messages { get; set; }

        [ConfigurationKeyName("max_tokens")]
        public int MaxTokens { get; set; }

        [ConfigurationKeyName("temperature")]
        public float Temperature { get; set; }

        [ConfigurationKeyName("top_p")]
        public float TopP { get; set; }

        public IEnumerable<ChatMessage> ChatMessages =>
            Messages.Select<Message, ChatMessage>(message => message.Role switch
            {
                "system" => ChatMessage.CreateSystemMessage(message.Content),
                "user" => ChatMessage.CreateUserMessage(message.Content),
                "assistant" => ChatMessage.CreateAssistantMessage(message.Content),
                _ => throw new ArgumentException($"Unknown role: {message.Role}", nameof(message.Role))
            });
    }
}
