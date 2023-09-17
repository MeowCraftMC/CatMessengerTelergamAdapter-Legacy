using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CatMessenger.Telegram.MessageComponent;

public class ChatMessage
{
    [JsonProperty("text")]
    public string? Text { get; set; }    // Todo: qyl27: I believe it must be a literal message!
    
    [JsonProperty("extra")]
    public List<ChatMessage>? Extra { get; set; }
    
    [JsonIgnore]
    [JsonConverter(typeof(StringEnumConverter))]
    public TextColor? TextColor { get; set; }
    
    [JsonIgnore]
    public string? HexColor { get; set; }
    
    [JsonProperty("color")]
    public string? Color
    {
        get => HexColor ?? (TextColor != null ? JsonConvert.SerializeObject(TextColor, new StringEnumConverter())
            .Replace("\"", "") : null);
        set => HexColor = value;
    }

    [JsonProperty("font")]
    public string? Font { get; set; }
    
    [JsonProperty("bold")]
    public bool? Bold { get; set; }
    
    [JsonProperty("italic")]
    public bool? Italic { get; set; }
    
    [JsonProperty("underlined")]
    public bool? Underlined { get; set; }
    
    [JsonProperty("strikethrough")]
    public bool? Strikethrough { get; set; }
    
    [JsonProperty("obfuscated")]
    public bool? Obfuscated { get; set; }
    
    [JsonProperty("insertion")]
    public string? Insertion { get; set; }
    
    [JsonProperty("clickEvent")]
    public ClickEventType? ClickEvent { get; set; }
    
    public class ClickEventType
    {
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClickAction Action { get; set; }
        
        [JsonProperty("value")]
        public string Value { get; set; }

        public ClickEventType(ClickAction action, string value)
        {
            Action = action;
            Value = value;
        }
        
        public enum ClickAction
        {
            [EnumMember(Value = "open_url")]
            OpenUrl = 0,
            
            [EnumMember(Value = "open_file")]
            [Obsolete]
            OpenFile = 1,
            
            [EnumMember(Value = "run_command")]
            RunCommand = 2,
            
            [EnumMember(Value = "suggest_command")]
            SuggestCommand = 3,
            
            [EnumMember(Value = "change_page")]
            [Obsolete]
            ChangePage = 4,
            
            [EnumMember(Value = "copy_to_clipboard")]
            CopyToClipboard = 5
        }
    }

    [JsonProperty("hoverEvent")]
    public HoverEventType? HoverEvent { get; set; }
    
    public class HoverEventType
    {
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HoverAction Action { get; set; }
        
        [JsonIgnore]
        public HoverContents Contents { get; set; }
        
        [JsonProperty("contents")]
        public object? ContentsText => Contents.ToObject();

        public HoverEventType(ChatMessage message)
        {
            Action = HoverAction.ShowText;
            Contents = new HoverContents(message);
        }

        public HoverEventType(HoverAction action, HoverContents contents)
        {
            Action = action;
            Contents = contents;
        }
        
        public enum HoverAction
        {
            [EnumMember(Value = "show_text")]
            ShowText = 0,
                
            [EnumMember(Value = "show_item")]
            [Obsolete]
            ShowItem = 1,
            
            [EnumMember(Value = "show_entity")]
            [Obsolete]
            ShowEntity = 2
        }
        
        public class HoverContents
        {
            [JsonIgnore]
            public ChatMessage? ShowText { get; set; }
            
            [JsonIgnore]
            [Obsolete]
            public string? ShowItem { get; set; }   // Todo: qyl27: Do we need to support it?
            
            [JsonIgnore]
            [Obsolete]
            public string? ShowEntity { get; set; } // Todo: qyl27: Do we need to support it?

            public HoverContents(ChatMessage message)
            {
                ShowText = message;
            }

            public object? ToObject()
            {
                return ShowText;
            }
        }
    }
    
    public ChatMessage()
    {
        Text = "";
    }

    public ChatMessage(string text)
    {
        Text = text;
    }

    public ChatMessage WithColor(TextColor color)
    {
        TextColor = color;
        return this;
    }
    
    public ChatMessage WithColor(string color)
    {
        HexColor = color;
        return this;
    }
    
    public ChatMessage Append(ChatMessage msg)
    {
        // Todo: qyl27: Throw empty components.
        // if (Text == null)
        // {
        //     return msg;
        // }
        
        if (Extra == null)
        {
            Extra = new List<ChatMessage>();
        }
        
        Extra.Add(msg);
        return this;
    }

    public ChatMessage WithExtra(params ChatMessage[] msg)
    {
        if (Extra == null)
        {
            Extra = new List<ChatMessage>();
        }
        
        Extra.AddRange(msg);
        return this;
    }

    public ChatMessage WithHoverText(string text)
    {
        HoverEvent = new HoverEventType(HoverEventType.HoverAction.ShowText, new HoverEventType.HoverContents(new ChatMessage(text)));
        return this;
    }
    
    public ChatMessage WithHoverMessage(ChatMessage message)
    {
        HoverEvent = new HoverEventType(HoverEventType.HoverAction.ShowText, new HoverEventType.HoverContents(message));
        return this;
    }

    public ChatMessage WithBold()
    {
        Bold = true;
        return this;
    }
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}