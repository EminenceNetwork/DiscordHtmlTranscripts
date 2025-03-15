# 🍿 DiscordHtmlTranscripts

![GitHub release (latest by date)](https://img.shields.io/github/v/release/YOUR_GITHUB/DiscordHtmlTranscripts)
![GitHub last commit](https://img.shields.io/github/last-commit/YOUR_GITHUB/DiscordHtmlTranscripts)
![GitHub license](https://img.shields.io/github/license/YOUR_GITHUB/DiscordHtmlTranscripts)
![Nuget](https://img.shields.io/nuget/v/DiscordHtmlTranscripts?label=NuGet)

**DiscordHtmlTranscripts** is a simple **C# library** for generating **beautiful, HTML-formatted Discord transcripts** from support tickets or chat logs. It is built for **Discord.Net** and works seamlessly with bot integrations.

> 💡 **Supports:** Messages, profile pictures, attachments, embeds, timestamps, and more!

---

## 🚀 **Installation**

### **NuGet Package**
You can install the library from **NuGet**:

```sh
dotnet add package DiscordHtmlTranscripts
```

OR using **Visual Studio NuGet Manager**.

### **Manual Build**
1. Clone the repository:
   ```sh
   git clone [https://github.com/EminenceNetwork/DiscordHtmlTranscripts.git](https://github.com/EminenceNetwork/DiscordHtmlTranscripts.git)
   ```
2. Build the project:
   ```sh
   dotnet build
   ```

---

## 📚 **Usage**

### **1️⃣ Generating a Transcript**
```csharp
using DiscordHtmlTranscripts;
using Discord.WebSocket;

public async Task GenerateTranscript(SocketTextChannel channel)
{
    string? transcriptPath = await TranscriptGenerator.GenerateHtmlTranscript(channel);
    if (transcriptPath != null)
    {
        Console.WriteLine($"Transcript saved at: {transcriptPath}");
    }
}
```

### **2️⃣ Handling `/closeticket` Command**
```csharp
[SlashCommand("closeticket", "Closes a support ticket and generates a transcript.")]
[RequireUserPermission(GuildPermission.ManageChannels)]
public async Task CloseTicket([Summary("channel")] ITextChannel channel)
{
    var transcriptPath = await TranscriptGenerator.GenerateHtmlTranscript((SocketTextChannel)channel);
    
    if (transcriptPath == null)
    {
        await RespondAsync("❌ No messages to save!", ephemeral: true);
        return;
    }

    var logChannel = Context.Guild.TextChannels.FirstOrDefault(c => c.Name.Contains("ticket-logs"));
    if (logChannel != null)
    {
        await logChannel.SendFileAsync(transcriptPath, "📜 Ticket Transcript");
    }

    await channel.DeleteAsync();
    await RespondAsync("✅ Ticket closed and transcript saved!", ephemeral: true);
}
```

---

## 📝 **Customization**
You can modify `template.html` to **change the design** of transcripts.

### **Example: Customize Message Format**
```html
<div class='message'>
    <div class='pfp'><img src="{AVATAR_URL}" alt="User Avatar"></div>
    <div class='message-content'>
        <span class='author'>{USERNAME}</span>
        <span class='timestamp'>{TIMESTAMP}</span>
        <p>{MESSAGE_CONTENT}</p>
    </div>
</div>
```

---

## 🎨 **Features**
✅ **Fetches all messages** (not just last 100)  
✅ **Includes user profile pictures**  
✅ **Supports attachments (images, files, videos)**  
✅ **Formats Discord embeds properly**  
✅ **Uses a customizable HTML template**  
✅ **Supports Markdown formatting**  

---

## 🛠 **API Reference**
### **`GenerateHtmlTranscript()`**
Generates an HTML transcript from a Discord text channel.

```csharp
Task<string?> GenerateHtmlTranscript(SocketTextChannel channel, string outputDir = "transcripts")
```
| Parameter    | Type               | Description                                |
|-------------|------------------|--------------------------------------------|
| `channel`   | `SocketTextChannel` | The Discord channel to fetch messages from |
| `outputDir` | `string` (Optional) | Directory to save the transcript file      |

**Returns:**  
✔️ `string?` (File path of the transcript)  
❌ `null` (If no messages were found)

---

## 🌟 **Planned Features**
📌 **PDF Export Support**  
📌 **Markdown to HTML Formatting**  
📌 **Custom Themes for Transcripts**  

---

## 🛠 **Contributing**
💡 Contributions are welcome! Feel free to fork the repo and submit a **pull request**.

```sh
git clone https://github.com/YOUR_GITHUB/DiscordHtmlTranscripts.git
cd DiscordHtmlTranscripts
dotnet build
```

---

## 📝 **License**
This project is licensed under the **MIT License**.

📜 **[Read License](LICENSE)**

---

## 📱 **Support & Contact**
👤 Created by **Kendon**  
📌 **GitHub:** [Your Repo](https://github.com/EminenceNetwork/DiscordHtmlTranscripts)  
📌 **Discord:** `c1tad31`  

---

🔥 **Made with ❤️ for the Discord community!** 🚀
