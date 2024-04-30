internal class Player
{
    public string Name { get; set; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int MaxHp { get; }
    public int Gold { get; set; }
    public int Current_Hp { get; set; }
    public Player(string name, string job, int level, int atk, int def, int maxhp, int gold, int current_hp)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxhp;
        Gold = gold;
        Current_Hp = current_hp;
    }
}