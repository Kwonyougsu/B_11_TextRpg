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

    

    public void QuestRewardTxt(string Txt)
    {
        Console.WriteLine($"보상 : {Txt}");
    }
    public void QuestRewardType(RewardType type)
    {
        switch (type)
        {
            case RewardType.GOLD1:
                QuestRewardTxt("1000G");
                break;
            case RewardType.GOLD2:
                QuestRewardTxt("2000G");
                break;
            case RewardType.GOLD3:
                QuestRewardTxt("3000G");
                break;
            case RewardType.GOLD4:
                QuestRewardTxt("4000G");
                break;
            case RewardType.ITEM1:
                QuestRewardTxt("무쇠방패");
                break;
            case RewardType.ITEM2:
                QuestRewardTxt("무쇠방패");
                break;
            case RewardType.ITEM3:
                QuestRewardTxt("무쇠방패");
                break;
            case RewardType.ITEM4:
                QuestRewardTxt("무쇠방패");
                break;
            case RewardType.ITEM5:
                QuestRewardTxt("무쇠방패");
                break;
            default:
                break;
        }

    }
}

internal class QuestClear
{
    public RewardType Type;
    GameManager gameManager;
    public int Count;
    public int QuestClearCheck;
    public void QuestCount(int count, ref int questClearCheck)
    {
        Count = count;
        questClearCheck = QuestClearCheck;
    }
    public void QuestClearRequest(RewardType type)       
    {
        switch (type)
        {
            case RewardType.GOLD1:
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");                   
                }
                else
                {
                    Console.WriteLine("아이템을 장착해 봅시다");
                }
                break;
            case RewardType.GOLD2:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.GOLD3:
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                }
                else
                {
                    Console.WriteLine("물약을 사용해 봅시다");
                }
                break;
            case RewardType.GOLD4:
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                }
                else
                {
                    Console.WriteLine("층을 올라가 봅시다");
                }
                break;
            case RewardType.ITEM1:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.ITEM2:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.ITEM3:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.ITEM4:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.ITEM5:
                if (Count >= 5 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            default:
                break;
        }
        
    }

    public void QuestClearRewardGold(RewardType type , Player player , int giveGold)
    {
        int gold = player.Gold;

            gold = giveGold + gold;
            player.SetGold(gold);

    }

    public class QuestListOrder
    {

        public static List<QuestList> questOrder = new List<QuestList>();
        static QuestListOrder()
        {
            QuestOrder();
        }

        public static void QuestOrder()
        {
            questOrder.Add(new QuestList("T1 아이템을 장착해봅시다", "아이템 장착하기", RewardType.GOLD1, false));
            questOrder.Add(new QuestList("T2 몬스터를 잡아봅시다", "아무몬스터나 5마리 사냥하기", RewardType.GOLD2, false));
            questOrder.Add(new QuestList("T3 물약을 사용해 봅시다", "물약사용하기", RewardType.GOLD3, false));
            questOrder.Add(new QuestList("T4 층을 올라가 봅시다", "몬스터를 사냥한뒤 층오르기", RewardType.GOLD4, false));
            questOrder.Add(new QuestList("아이템을 장착하자5", "아이템 장착하기", RewardType.ITEM1, false));
        }
    }
}



