using System.Runtime.Serialization;

namespace CatMessenger.Telegram.Utilities.MessageComponent;

public enum TextColor
{
    [EnumMember(Value = "black")]
    Black,

    [EnumMember(Value = "dark_blue")]
    DarkBlue,

    [EnumMember(Value = "dark_green")]
    DarkGreen,

    [EnumMember(Value = "dark_aqua")]
    DarkAqua,

    [EnumMember(Value = "dark_red")]
    DarkRed,

    [EnumMember(Value = "dark_purple")]
    DarkPurple,

    [EnumMember(Value = "gold")]
    Gold,

    [EnumMember(Value = "gray")]
    Gray,

    [EnumMember(Value = "dark_gray")]
    DarkGray,

    [EnumMember(Value = "blue")]
    Blue,

    [EnumMember(Value = "green")]
    Green,

    [EnumMember(Value = "aqua")]
    Aqua,

    [EnumMember(Value = "red")]
    Red,

    [EnumMember(Value = "light_purple")]
    LightPurple,

    [EnumMember(Value = "yellow")]
    Yellow,

    [EnumMember(Value = "white")]
    White,

    [EnumMember(Value = "reset")]
    Reset
}