using CatMessenger.Telegram.Utilities.MessageComponent;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CatMessenger.Telegram.Utilities;

public class MessageParser
{
    public static ChatMessage FromUpdate(Update update)
    {
        return update.Type switch
        {
            UpdateType.Unknown => FromUnknown(),
            UpdateType.Message => FromMessage(update.Message!),
            UpdateType.InlineQuery => FromUnsupported(update.Type),
            UpdateType.ChosenInlineResult => FromUnsupported(update.Type),
            UpdateType.CallbackQuery => FromUnsupported(update.Type),
            UpdateType.EditedMessage => FromMessage(update.EditedMessage!, true),
            UpdateType.ChannelPost => FromMessage(update.ChannelPost!),
            UpdateType.EditedChannelPost => FromMessage(update.EditedChannelPost!, true),
            UpdateType.ShippingQuery => FromUnsupported(update.Type),
            UpdateType.PreCheckoutQuery => FromUnsupported(update.Type),
            UpdateType.Poll => FromUnsupported(update.Type),
            UpdateType.PollAnswer => FromUnsupported(update.Type),
            UpdateType.MyChatMember => FromUnsupported(update.Type),
            UpdateType.ChatMember => FromUnsupported(update.Type),
            UpdateType.ChatJoinRequest => FromUnsupported(update.Type),
            _ => FromUnsupported(update.Type)
        };
    }

    public static ChatMessage FromMessage(Message message, bool edited = false)
    {
        var chatMsg = new ChatMessage();

        if (message.From != null)
        {
            chatMsg.Append(new ChatMessage("[")
                .WithColor(TextColor.Aqua)
                .Append(FromUser(message.From))
                .Append(new ChatMessage("] ")));
        }

        if (edited)
        {
            chatMsg.Append(new ChatMessage("[已编辑] ").WithColor(TextColor.Green));
        }
        
        if (message.ReplyToMessage != null)
        {
            var reply = message.ReplyToMessage;
            chatMsg.Append(new ChatMessage("[回复 ")
                .WithColor(TextColor.Gold)
                .Append(FromUser(reply.From!))
                .Append(new ChatMessage("："))
                .Append(FromText(reply.Text!, 10))
                .Append(new ChatMessage("] ")));
        }

        if (message.ForwardFrom != null)
        {
            var forwardFrom = message.ForwardFrom;
            chatMsg.Append(new ChatMessage("[转发自用户 ")
                .WithColor(TextColor.Gold)
                .Append(FromUser(forwardFrom))
                .Append(new ChatMessage("] ")));
        }

        if (message.ForwardFromChat != null)
        {
            var forwardFrom = message.ForwardFromChat;
            chatMsg.Append(new ChatMessage("[转发")
                .WithColor(TextColor.Gold)
                .WithHoverMessage(FromChat(forwardFrom))
                .Append(new ChatMessage("] ")));
        }

        if (message.Photo is { Length: > 0 })
        {
            chatMsg.Append(new ChatMessage("[图片] ").WithColor(TextColor.Green));
            
            // Fixme: qyl27: Show pictures count?
            // var count = message.Photo.DistinctBy(photo => photo.FileUniqueId).Count();
            // if (count == 1)
            // {
            //     chatMsg.Append(new ChatMessage("[图片] ").WithColor(TextColor.Green));
            // }
            // else
            // {
            //     chatMsg.Append(new ChatMessage($"[图片 * {count}] ").WithColor(TextColor.Green));
            // }
        }

        if (message.Sticker != null)
        {
            chatMsg.Append(FromSticker(message.Sticker));
        }

        if (message.Document != null)
        {
            chatMsg.Append(new ChatMessage($"[文件 {message.Document.FileName}] ").WithColor(TextColor.Blue));
        }
        
        if (message.Voice != null)
        {
            chatMsg.Append(new ChatMessage($"[语音 {message.Voice.Duration}秒] ").WithColor(TextColor.Blue));
        }
        
        if (message.Audio != null)
        {
            chatMsg.Append(new ChatMessage($"[音频 {message.Audio.Duration}秒] ").WithColor(TextColor.Blue));
        }
        
        if (message.Video != null)
        {
            chatMsg.Append(new ChatMessage($"[视频 {message.Video.Duration}秒] ").WithColor(TextColor.Blue));
        }

        if (message.Text != null)
        {
            chatMsg.Append(FromText(message.Text, 30));
        }

        if (message.Caption != null)
        {
            chatMsg.Append(FromText(message.Caption, 30));
        }
        
        return chatMsg;
    }

    public static ChatMessage FromSticker(Sticker sticker)
    {
        return new ChatMessage($"[贴纸 {sticker.Emoji}] ")
            .WithColor(TextColor.Blue)
            .WithHoverText($"来自贴纸包 {sticker.SetName}");
    }

    public static ChatMessage FromChat(Chat chat)
    {
        var message = new ChatMessage();

        switch (chat.Type)
        {
            case ChatType.Channel:
            case ChatType.Supergroup or ChatType.Group:
                message.Append(new ChatMessage(chat.Title!).WithBold());
                break;
            case ChatType.Private or ChatType.Sender:
                message.Append(FromUser(chat.FirstName!, chat.LastName, chat.Username, true));
                break;
        }

        return message;
    }

    public static ChatMessage FromText(string originText, int trim = -1, bool hoverShowFull = true)
    {
        var message = new ChatMessage();

        if (string.IsNullOrWhiteSpace(originText))
        {
            return message;
        }
        
        var text = originText.Replace('\n', ' ');

        if (trim > 0 && text.Length > trim)
        {
            message.Append(new ChatMessage(text[..trim] + "……"));

            if (hoverShowFull)
            {
                message
                    .Append(new ChatMessage("[全文] ")
                        .WithColor(TextColor.Gold)
                        .WithHoverText(text));
            }
        }
        else
        {
            message.Append(new ChatMessage(text));
        }

        return message;
    }

    public static ChatMessage FromUser(User user)
    {
        return FromUser(user.FirstName, user.LastName, user.Username);
    }
    
    private static ChatMessage FromUser(string firstName, string? lastName, string? username, bool noHover = false)
    {
        var message = new ChatMessage(firstName);

        if (lastName != null)
        {
            message.Append(new ChatMessage(" " + lastName));
        }

        if (username != null)
        {
            if (noHover)
            {
                message.Append(new ChatMessage($"(@{username})"));
            }
            else
            {
                message.WithHoverText($"@{username}");
            }
        }

        return message;
    }

    public static ChatMessage FromUnsupported(UpdateType type)
    {
        return new ChatMessage($"[不支持的消息 {type}] ").WithColor(TextColor.DarkAqua);
    }
    
    public static ChatMessage FromUnknown()
    {
        return new ChatMessage("[未知消息] ").WithColor(TextColor.DarkAqua);
    }
}