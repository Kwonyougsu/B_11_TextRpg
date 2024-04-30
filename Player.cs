internal class Player
{
    public string Name { get; set; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; }
    public int Gold { get; set; }
    public int Attacked_Hp{ get; set; }
    public Player(string name, string job, int level, int atk, int def, int hp, int gold, int attacked_Hp)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
        Attacked_Hp = attacked_Hp;
    }
}

