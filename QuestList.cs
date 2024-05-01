public enum RewardType
{
    GOLD,
    ITEM

}
internal class QuestList
{
    public string QuestName { get; }
    public string QuestDetail { get; }

    public RewardType Type {  get; }

    public bool IsAccept { get; set; }


    public QuestList(string questname, string questdetail, RewardType type, bool isAccept = false)
    {
        QuestName = questname;
        QuestDetail = questdetail;
        Type = type;
        isAccept = IsAccept;
    }

    public QuestContents(RewardType type)
    {
        Type = type;
        if(type == RewardType.ITEM) 
        {
            Console.WriteLine("골드");
        }
        
        if(type == RewardType.GOLD) 
        {
            Console.WriteLine("아이템");
        }
    }

}

