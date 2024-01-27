public enum ChatEmoteType
{
    FiveHead,
    Jebaited,
    Lul,
    PepeLaugh,
    Kekw,
    Kappa,
    Monkaw,
    NotLikeThis,
    Poggers
}

public static class ChatEmotes
{
    private const string EMOTE_SHEET_NAME = "Emotes";
    
    private const string FIVE_HEAD = "Emotes_5Head";
    private const string Jebaited = "Emotes_Jebaited";
    private const string Lul = "Emotes_LUL";
    private const string PepeLaugh = "Emotes_PepeLaugh";
    private const string Kekw = "Emotes_KEKW";
    private const string Kappa = "Emotes_Kappa";
    private const string Monkaw = "Emotes_MonkaW";
    private const string NotLikeThis = "Emotes_NotLikeThis";
    private const string Poggers = "Emotes_Poggers";

    private static string GetEmote(string emoteName)
    {
        return $"<sprite=\"{EMOTE_SHEET_NAME}\" name=\"{emoteName}\">";
    }
    
    public static string GetEmote(ChatEmoteType type)
    {
        return type switch
        {
            ChatEmoteType.FiveHead => GetEmote(FIVE_HEAD),
            ChatEmoteType.Jebaited => GetEmote(Jebaited),
            ChatEmoteType.Lul => GetEmote(Lul),
            ChatEmoteType.PepeLaugh => GetEmote(PepeLaugh),
            ChatEmoteType.Kekw => GetEmote(Kekw),
            ChatEmoteType.Kappa => GetEmote(Kappa),
            ChatEmoteType.Monkaw => GetEmote(Monkaw),
            ChatEmoteType.NotLikeThis => GetEmote(NotLikeThis),
            ChatEmoteType.Poggers => GetEmote(Poggers),
            _ => string.Empty
        };
    }
}
