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
                QuestRewardTxt("베테랑의 상징");
                break;
            case RewardType.ITEM5:
                QuestRewardTxt("장로드래곤의 뿔");
                break;
            default:
                break;
        }

    }
}

internal class QuestClear
{

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
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("방어력을 20이상 달성하기");
                }
                break;
            case RewardType.ITEM2:
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                }
                else
                {
                    Console.WriteLine("공격력을 30이상 달성하기");
                }
                break;
            case RewardType.ITEM3:
                if (QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                }
                else
                {
                    Console.WriteLine("4층까지 도달해봅시다");
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
                    Console.WriteLine("3층이상의몬스터를 사냥해봅시다!");
                    Console.WriteLine($"{Count} / 5");
                }
                break;
            case RewardType.ITEM5:
                if (Count >= 1 || QuestClearCheck == 1)
                {
                    Console.WriteLine("조건 완료!");
                    QuestClearCheck = 1;
                }
                else
                {
                    Console.WriteLine("3층의 장로드래곤을 한마리 쓰러뜨려주세요!");
                    Console.WriteLine($"{Count} / 1");
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
            questOrder.Add(new QuestList("Q1 방어력 20을 달성하자", "방어력을 20까지 올려봅시다", RewardType.ITEM1, false));
            questOrder.Add(new QuestList("Q2 공격력 30을 달성하자", "공격력을 30까지 올려봅시다", RewardType.ITEM2, false));
            questOrder.Add(new QuestList("Q3 4층에 도달하자", "4층까지 진행해봅시다!", RewardType.ITEM3, false));
            questOrder.Add(new QuestList("Q4 Veteran", "많은 몬스터를 해치우고 도달한 마지막층입니다 당신을 증명하세요", RewardType.ITEM4, false));
            questOrder.Add(new QuestList("Q5 Kill The Dragon", "용을 잡고 당신의 한계에 도전하세요", RewardType.ITEM5, false));
        }
    }
}



