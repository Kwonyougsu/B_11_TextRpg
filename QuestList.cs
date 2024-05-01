public enum RewardType
{
    GOLD1,
    GOLD2,
    GOLD3,
    GOLD4,
    ITEM1,
    ITEM2,
    ITEM3,
    ITEM4,
    ITEM5,
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
        if (type == RewardType.GOLD1)
        {
            Console.WriteLine("보상 : 골드");
        }
        else if (type == RewardType.ITEM1)
        {
            Console.WriteLine("보상 : 가시방패");
        }
    }
}

internal class QuestClear
{
    public RewardType Type;

    


    public void QuestClearReward(RewardType type , Player player , int giveGold)
    {
        int gold = player.Gold;
        Console.WriteLine(gold);
        if (type == RewardType.GOLD1)
        {
            gold = giveGold + gold;
            player.SetGold(gold);
        }
        else if (type == RewardType.GOLD2)
        {
            Console.WriteLine("보상 : 가시방패");
        }
    }
}

