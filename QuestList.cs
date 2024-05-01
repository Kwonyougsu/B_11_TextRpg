using Team_B_11_RPG;

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
            Console.WriteLine("보상 : 100 G");
        }
        else if (type == RewardType.GOLD2)
        {
            Console.WriteLine("보상 : 200 G");
        }
        else if (type == RewardType.GOLD3)
        {
            Console.WriteLine("보상 : 300 G");
        }
        else if (type == RewardType.GOLD4)
        {
            Console.WriteLine("보상 : 400 G");
        }
        else if (type == RewardType.ITEM1)
        {
            Console.WriteLine("보상 : 무쇠갑옷");
        }
        else if (type == RewardType.ITEM2)
        {
            Console.WriteLine("보상 : 무쇠갑옷");
        }
        else if (type == RewardType.ITEM3)
        {
            Console.WriteLine("보상 : 무쇠갑옷");
        }
        else if (type == RewardType.ITEM4)
        {
            Console.WriteLine("보상 : 무쇠갑옷");
        }
        else if (type == RewardType.ITEM5)
        {
            Console.WriteLine("보상 : 무쇠갑옷");
        }

    }
}

internal class QuestClear
{
    public RewardType Type;
    GameManager gameManager;
    public int Count;
    public int questClearCheck;

    public void QuestCount(int Count, int questClearCheck)
    {
        
    }
    public void QuestClearRequest(RewardType type)
        
    {
        if (type == RewardType.GOLD1)
        {           
            Console.WriteLine("아이템을 장착해봅시다!");            
        }
        else if (type == RewardType.GOLD2)
        {
            Console.WriteLine("몬스터를 사냥해봅시다!");
            Console.WriteLine($"{Count} / 5");
            if(Count > 5)
            {
                Console.WriteLine("조건 완료!");
                Count = 0;
                questClearCheck = 1;
            }
        }
        else if (type == RewardType.GOLD3)
        {
            Console.WriteLine("레벨업을 해봅시다!");
        }
        else if (type == RewardType.GOLD4)
        {
            Console.WriteLine("장비를 팔아봅시다");
        }
        else if (type == RewardType.ITEM1)
        {
            Console.WriteLine("이건뭘로할까");
        }
        else if (type == RewardType.ITEM2)
        {
            Console.WriteLine("이건뭘로할까2");
        }
        else if (type == RewardType.ITEM3)
        {
            Console.WriteLine("이건뭘로할까3");
        }
        else if (type == RewardType.ITEM4)
        {
            Console.WriteLine("이건뭘로할까4");
        }
        else if (type == RewardType.ITEM5)
        {
            Console.WriteLine("이건뭘로할까5");
        }
    }

    public void QuestClearRewardGold(RewardType type , Player player , int giveGold)
    {
        int gold = player.Gold;
        Console.WriteLine(gold);

            gold = giveGold + gold;
            player.SetGold(gold);

    }

    
}



