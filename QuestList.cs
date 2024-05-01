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

    
}
internal class QuestReward
{
    public RewardType Type;



    public void QuestRewardType(RewardType type)
    {
        if (type == RewardType.GOLD)
        {
            Console.WriteLine("보상 : 골드");
        }
        else if (type == RewardType.ITEM)
        {
            Console.WriteLine("보상 : 아이템");
        }
    }
}

