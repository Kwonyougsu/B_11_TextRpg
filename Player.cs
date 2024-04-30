using System.Numerics;
using Team_B_11_RPG;

internal class Player
{
    private int PlayerExp = 0;
    public string Name { get; set; }
    public string Job { get; }
    public int Level { get; }
    public double Atk { get; set; }
    public int Def { get; set; }
    public int MaxHp { get; set; }
    public int Gold { get; set; }
    public int Current_Hp { get; set; }
    public int Exp { get; set; }
    public Player(string name, string job, int level, double atk, int def, int maxhp, int gold, int current_hp, int exp)
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
    }
    
    public void GetExp(int monsterexp)
    {
        PlayerExp += monsterexp;
    }
    public void PlayerLevelUp() 
    {
        
        Console.WriteLine(PlayerExp);
        Console.WriteLine("[내 정 보]");
        Console.WriteLine($"Lv.{Level} {Name} ({Job})");
        Console.WriteLine($"HP :{MaxHp} -> {Current_Hp}");
        PlayerExp = 0;
    }

}