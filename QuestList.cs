public enum RewardType
{
    GOLD,
    ITEM
}
internal class QuestList
{
    public string QuestName { get; }
    public string QuestDetail { get; }

    private RewardType Type;

    public bool IsClear { get; set; }


    public QuestList(string questname, string questdetail, RewardType type, bool isClear = false)
    {
        QuestName = questname;
        QuestDetail = questdetail;
        Type = type;
        isClear = IsClear;
    }
}

