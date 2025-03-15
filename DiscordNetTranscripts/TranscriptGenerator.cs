using System.Text;
using Discord;
using Discord.WebSocket;

namespace DiscordHtmlTranscripts
{
    public class TranscriptGenerator
    {
        private const string TemplatePath = "template.html"; // Ensure this file exists

        public static async Task<string?> GenerateHtmlTranscript(SocketTextChannel channel, string outputDir = "transcripts")
        {
            if (!File.Exists(TemplatePath))
            {
                Console.WriteLine("❌ Error: Template file not found.");
                return null;
            }

            var allMessages = await GetAllMessages(channel);
            if (allMessages.Count == 0) return null;

            // Load the HTML template
            string htmlTemplate = await File.ReadAllTextAsync(TemplatePath);

            // Reverse messages (so they are in oldest-to-newest order)
            allMessages.Reverse();

            // Build message logs
            var messageLogs = new StringBuilder();
            foreach (var message in allMessages)
            {
                var user = message.Author as SocketUser;
                var avatarUrl = user?.GetAvatarUrl() ?? user?.GetDefaultAvatarUrl() ?? ""; // Get avatar or default Discord avatar

                messageLogs.Append($@"
                <div class='message'>
                    <div class='pfp'>
                        <img src='{avatarUrl}' alt='User Avatar'>
                    </div>
                    <div class='message-content'>
                        <span class='author'>{message.Author.Username}</span>
                        <span class='timestamp'>[{message.Timestamp:yyyy-MM-dd HH:mm:ss}]</span>
                        <div class='text'>{message.Content ?? "[No Text]"}</div>"); // ✅ FIX: Prevents null error

                // Attachments (images, files)
                if (message.Attachments.Count > 0)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        if (attachment.Filename.EndsWith(".png") || attachment.Filename.EndsWith(".jpg") ||
                            attachment.Filename.EndsWith(".jpeg") || attachment.Filename.EndsWith(".gif"))
                        {
                            messageLogs.Append($"<div class='attachment'><img src='{attachment.Url}'></div>");
                        }
                        else
                        {
                            messageLogs.Append($"<p><a href='{attachment.Url}' target='_blank'>{attachment.Filename}</a></p>");
                        }
                    }
                }

                // Embeds (e.g., bot messages, rich content)
                if (message.Embeds.Count > 0)
                {
                    foreach (var embed in message.Embeds)
                    {
                        messageLogs.Append($@"
                        <div class='embed'>
                            <p><strong>{embed.Title ?? "Embed"}</strong></p>
                            <p>{embed.Description ?? ""}</p>");
                        if (!string.IsNullOrEmpty(embed.Url))
                            messageLogs.Append($"<p><a href='{embed.Url}' target='_blank'>View Link</a></p>");
                        messageLogs.Append("</div>");
                    }
                }

                messageLogs.Append("</div></div>"); // Close .message-content and .message
            }

            // Replace the placeholder in the template with the actual messages
            string finalHtml = htmlTemplate.Replace("<!-- Messages will be inserted here -->", messageLogs.ToString());

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDir);
            var filePath = Path.Combine(outputDir, $"ticket-{channel.Id}.html");
            await File.WriteAllTextAsync(filePath, finalHtml);

            return filePath;
        }

        // ✅ Fetches **all** messages in the channel
        private static async Task<List<IMessage>> GetAllMessages(SocketTextChannel channel)
        {
            var messages = new List<IMessage>();
            IMessage lastMessage = null;

            try
            {
                while (true)
                {
                    var fetchedMessages = lastMessage == null
                        ? await channel.GetMessagesAsync(100).FlattenAsync()
                        : await channel.GetMessagesAsync(lastMessage, Direction.Before, 100).FlattenAsync();

                    if (!fetchedMessages.Any()) break;

                    messages.AddRange(fetchedMessages);
                    lastMessage = fetchedMessages.Last();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching messages: {ex.Message}");
            }

            return messages;
        }
    }
}
