using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Team_B_11_RPG;

internal class Player
{
    private int PlayerExp = 0;
    private int MaxExp = 0;
    private int[] requireExp = {0,10, 35, 65, 100 };

    Random random = new Random();

    public static List<Item> inventory;

    public string Name { get; set; }
    public string Job { get; }
    public int Level { get; set; }
    public double Atk { get; set; }
    public int Def { get; set; }
    public int MaxHp { get; set; }
    public int Gold { get; set; }
    public int Current_Hp { get; set; }
    public int Exp { get; set; }
    public int Postion { get; set; }
    public Player(string name, string job, int level, double atk, int def, int maxhp, int gold, int current_hp, int exp,int postion)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxhp;
        Gold = gold;
        Current_Hp = current_hp;
        Exp = exp;
        Postion = postion;
    }
    
    public void GetExp(int monsterexp)
    {
        PlayerExp += monsterexp;
    }
    public void PlayerLevelUp() 
    {
        int LevelUpExp = requireExp[Level]; 
        //MaxExp를 만들어서 넣고 초기화? 하는 방향성으로 ㄱㄱ 
        MaxExp = Exp + PlayerExp;
        if (MaxExp >= LevelUpExp) 
        { 
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job}) -> Lv.{Level+1} {Name} ({Job})");
            MaxHp = MaxHp + 20;
            Console.WriteLine("레벨업!");
            Console.WriteLine("체력이 회복되었습니다");
            Console.WriteLine($"HP :{MaxHp} -> {MaxHp}");
            Level = Level + 1;
            MaxExp = MaxExp - LevelUpExp;
            Exp = MaxExp;
            Current_Hp = MaxHp;
        }
        else
        {
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{MaxHp} -> {Current_Hp}");
            Console.WriteLine($"Exp :{Exp} -> {MaxExp}");
            Exp = Exp+ PlayerExp;
        }
        PlayerExp = 0;

        
    }    
    public void GetGold(int gold) 
    {
        Gold = gold;       
    }
    public void DungeonResult()
    {
        int gold = random.Next(100, 500);
        int postion = random.Next(0, 2);
        int itemdrop = random.Next(0, 10);
        int count = 1;
        Console.WriteLine("");
        Console.WriteLine("[획득 아이템]");
        ConsoleUtility.ShowTitle($"{gold} G");
        Gold = Gold + gold;
        if (postion != 0)
        {
            Console.Write("포션 - ");
            ConsoleUtility.ShowTitle($"{postion}");
            Postion = Postion + postion;
        }
        if (itemdrop < 5)
        {
            Console.Write("부식된 검");
            inventory.Add(new Item("부식된 검", "세월이 흐름에 따라 부식된 검.", ItemType.WEAPON, 2, 0, 0, 1000));
            ConsoleUtility.ShowTitle($"{count}");
        }
        
    }

    public void SetGold(int setGold)
    {
        Gold = setGold;
    }
}
