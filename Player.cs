using Newtonsoft.Json;
using System.Numerics;
using Team_B_11_RPG;

internal class Player
{
    private int PlayerExp = 0;
    private int MaxExp = 0;
    private int[] requireExp = { 0, 10, 35, 65, 100 };
    public int floor = 1;
    Random random = new Random();

    public static List<Item> inventory { get; set; }

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
    public int MaxMp { get; set; }
    public int Current_Mp { get; set; }

    public Player(string name, string job, int level, double atk, int def, int maxhp, int gold, int current_hp, int exp, int postion, int maxmp, int current_mp)
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
        MaxMp = maxmp;
        Current_Mp =current_mp;
    }

    public void Save(string filePath)
    {
        string json = JsonConvert.SerializeObject(this);
        File.WriteAllText(filePath, json);
    }

    public static Player Load(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Player>(json);
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
            Console.WriteLine($"Lv.{Level} {Name} ({Job}) -> Lv.{Level + 1} {Name} ({Job})");
            MaxHp = MaxHp + 20;
            MaxMp = MaxMp + 20;
            Console.WriteLine("레벨업!");
            Console.WriteLine("체력이 회복되었습니다");
            Console.WriteLine($"HP :{MaxHp} -> {MaxHp}");
            Console.WriteLine($"마나가 회복되었습니다");
            Console.WriteLine($"MP :{MaxMp} -> {MaxMp}");
            Level = Level + 1;
            MaxExp = MaxExp - LevelUpExp;
            Exp = MaxExp;
            Current_Hp = MaxHp;
            Current_Mp=MaxMp;
        }
        else
        {
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{MaxHp} -> {Current_Hp}");
            Console.WriteLine($"MP :{MaxMp} -> {Current_Mp}");
            Console.WriteLine($"Exp :{Exp} -> {MaxExp}");
            Exp = Exp + PlayerExp;
        }
        PlayerExp = 0;
    }

    public void GetGold(int gold)
    {
        Gold = gold;
    }

    public void SetGold(int setGold)
    {
        Gold = setGold;
    }
    public void DungeonResult()
    {

        if (floor == 1) 
        {
            int gold = random.Next(100, 500);
            int postion = random.Next(0, 2);
            int itemdrop = random.Next(0, 10); //50%확률로 아이템 드랍

            List<Item> monsterDropList = RandomMonster.monsterdrop.GetRange(0, 3);

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
                if (monsterDropList.Count > 0)
                {
                    int randomIndex = random.Next(0, monsterDropList.Count);
                    Item droppedItem = monsterDropList[randomIndex];
                    inventory.Add(droppedItem);
                    Console.Write($"{droppedItem.Name} - ");
                    ConsoleUtility.ShowTitle("1");
                }
            }
        }// 1층
        if (floor == 2) 
        {
            int gold = random.Next(500, 700);
            int postion = random.Next(0, 2);
            int itemdrop = random.Next(0, 10); //50%확률로 아이템 드랍

            List<Item> monsterDropList = RandomMonster.monsterdrop.GetRange(4, 7);

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
                if (monsterDropList.Count > 0)
                {
                    int randomIndex = random.Next(0, monsterDropList.Count);
                    Item droppedItem = monsterDropList[randomIndex];
                    inventory.Add(droppedItem);
                    Console.Write($"{droppedItem.Name} - ");
                    ConsoleUtility.ShowTitle("1");
                }
            }
        }// 2층
        if (floor == 3) 
        {
            int gold = random.Next(500, 700);
            int postion = random.Next(0, 2);
            int itemdrop = random.Next(0, 10); //50%확률로 아이템 드랍

            List<Item> monsterDropList = RandomMonster.monsterdrop.GetRange(8, 11);

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
                if (monsterDropList.Count > 0)
                {
                    int randomIndex = random.Next(0, monsterDropList.Count);
                    Item droppedItem = monsterDropList[randomIndex];
                    inventory.Add(droppedItem);
                    Console.Write($"{droppedItem.Name} - ");
                    ConsoleUtility.ShowTitle("1");
                }
            }
        }// 3층
        if (floor == 4) 
        {
            int gold = random.Next(500, 700);
            int postion = random.Next(0, 2);
            int itemdrop = random.Next(0, 10); //50%확률로 아이템 드랍

            List<Item> monsterDropList = RandomMonster.monsterdrop.GetRange(12, 15);

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
                if (monsterDropList.Count > 0)
                {
                    int randomIndex = random.Next(0, monsterDropList.Count);
                    Item droppedItem = monsterDropList[randomIndex];
                    inventory.Add(droppedItem);
                    Console.Write($"{droppedItem.Name} - ");
                    ConsoleUtility.ShowTitle("1");
                }
            }
        }// 4층 
    }
    public void DungeonFloor()
    {
        if(floor == 1) 
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle ! ");
            // 저장되어있는 랜덤몬스터 클리어 
            RandomMonster.randmonsters.Clear();
            // 랜덤 몬스터 생성
            Random randnumber = new Random();
            // 몬스터 출력
            for (int i = 0; i < randnumber.Next(1, 5); i++)
            {
                int j = randnumber.Next(0, 3);
                Monster.MakeMonster();
                Console.WriteLine($"Lv.{Monster.monsters[j].Level} {Monster.monsters[j].Name} HP {Monster.monsters[j].Hp} " +
                $" || 공격력 : {Monster.monsters[j].Atk} 방어력 : {Monster.monsters[j].Def}");
                Monster randomMonster = Monster.monsters[j];
                RandomMonster.randmonsters.Add(new RandomMonster(randomMonster.Name, randomMonster.Level, randomMonster.Atk, randomMonster.Def, randomMonster.Hp, randomMonster.Type, randomMonster.IsAlive));
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{Current_Hp}/{MaxHp}");
            Console.WriteLine($"MP :{Current_Mp}/{MaxMp}");
        }// 1층
        if (floor == 2) 
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle ! ");
            // 저장되어있는 랜덤몬스터 클리어 
            RandomMonster.randmonsters.Clear();
            // 랜덤 몬스터 생성
            Random randnumber = new Random();
            // 몬스터 출력
            for (int i = 0; i < randnumber.Next(1, 6); i++)
            {
                int j = randnumber.Next(3, 6);
                Monster.MakeMonster();
                Console.WriteLine($"Lv.{Monster.monsters[j].Level} {Monster.monsters[j].Name} HP {Monster.monsters[j].Hp} " +
                $" || 공격력 : {Monster.monsters[j].Atk} 방어력 : {Monster.monsters[j].Def}");
                Monster randomMonster = Monster.monsters[j];
                RandomMonster.randmonsters.Add(new RandomMonster(randomMonster.Name, randomMonster.Level, randomMonster.Atk, randomMonster.Def, randomMonster.Hp, randomMonster.Type, randomMonster.IsAlive));
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{Current_Hp}/{MaxHp}");
            Console.WriteLine($"MP :{Current_Mp}/{MaxMp}");

        }// 2층
        if (floor == 3) 
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle ! ");
            // 저장되어있는 랜덤몬스터 클리어 
            RandomMonster.randmonsters.Clear();
            // 랜덤 몬스터 생성
            Random randnumber = new Random();
            // 몬스터 출력
            for (int i = 0; i < randnumber.Next(1, 7); i++)
            {
                int j = randnumber.Next(6, 9);
                Monster.MakeMonster();
                Console.WriteLine($"Lv.{Monster.monsters[j].Level} {Monster.monsters[j].Name} HP {Monster.monsters[j].Hp} " +
                $" || 공격력 : {Monster.monsters[j].Atk} 방어력 : {Monster.monsters[j].Def}");
                Monster randomMonster = Monster.monsters[j];
                RandomMonster.randmonsters.Add(new RandomMonster(randomMonster.Name, randomMonster.Level, randomMonster.Atk, randomMonster.Def, randomMonster.Hp, randomMonster.Type, randomMonster.IsAlive));
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{Current_Hp}/{MaxHp}");
            Console.WriteLine($"MP :{Current_Mp}/{MaxMp}");

        }// 3층
        if (floor == 4) 
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("Battle ! ");
            // 저장되어있는 랜덤몬스터 클리어 
            RandomMonster.randmonsters.Clear();
            // 랜덤 몬스터 생성
            Random randnumber = new Random();
            // 몬스터 출력
            for (int i = 0; i < randnumber.Next(1, 2); i++)
            {
                int j = randnumber.Next(9, 12);
                Monster.MakeMonster();
                Console.WriteLine($"Lv.{Monster.monsters[j].Level} {Monster.monsters[j].Name} HP {Monster.monsters[j].Hp} " +
                $" || 공격력 : {Monster.monsters[j].Atk} 방어력 : {Monster.monsters[j].Def}");
                Monster randomMonster = Monster.monsters[j];
                RandomMonster.randmonsters.Add(new RandomMonster(randomMonster.Name, randomMonster.Level, randomMonster.Atk, randomMonster.Def, randomMonster.Hp, randomMonster.Type, randomMonster.IsAlive));
            }
            Console.WriteLine("");
            Console.WriteLine("[내 정 보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP :{Current_Hp}/{MaxHp}");
            Console.WriteLine($"MP :{Current_Mp}/{MaxMp}");

        }// 4층 (보스)

    }
}