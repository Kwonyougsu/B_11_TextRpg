using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Team_B_11_RPG
{
    public enum MonsterType
    {
        small = 1,
        buff = 2,
        epic = 3,
        boss = 4
    }
    internal class RandomMonster
    {
        public static List<RandomMonster> randmonsters = new List<RandomMonster>();
        public static List<Item> monsterdrop = new List<Item>();
        public string Name { get; set; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; }
        public int Hp { get; set; }
        public bool IsAlive { get; set; }
        public MonsterType Type { get; }
        public RandomMonster(string name, int level, int atk, int def, int hp, MonsterType type, bool IsAlive)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Type = type;
            this.IsAlive = true;
        }
        public void MonsterBattle(bool withNumber = false, int idx = 1)
        {
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} .", idx);
                Console.ResetColor();
            }
            if (!IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lv.{Level} {Name} Dead");
                Console.ResetColor();
            }
            if (IsAlive)
            {
                Console.Write($"Lv. {(Atk >= 0 ? "" : "")}{Level} ");
                Console.Write($"{Name}  ");
                Console.Write($"방어력 {(Def >= 0 ? "" : "")}{Def} ");
                Console.Write($"HP {(Hp >= 0 ? "" : "")}{Hp}");
                Console.WriteLine("");
            }

        }
        internal void IsAliveToggle()
        {
            IsAlive = !IsAlive;
        }
        public static void MonsterDropTable()
        {// 0~3 낡은 4~7 고급 8~11 빛나는 12 ~ 15 지배자 
            monsterdrop.Add(new Item("낡은 단검", "낡은 단검.", ItemType.WEAPON, 3, 0, 0, 1000));
            monsterdrop.Add(new Item("낡은 대검", "낡은 대검.", ItemType.WEAPON, 3, 0, 0, 1000));
            monsterdrop.Add(new Item("낡은 갑옷", "낡은 갑옷.", ItemType.ARMOR, 0, 3, 0, 1000));
            monsterdrop.Add(new Item("낡은 투구", "낡은 대검.", ItemType.ARMOR, 0, 2, 0, 1000));

            monsterdrop.Add(new Item("고급 단검", "고급스러운 단검, 날카롭다", ItemType.WEAPON, 3, 0, 0, 10000));
            monsterdrop.Add(new Item("고급 대검", "고급스러운 대검, 크기가 크며 묵직하다", ItemType.WEAPON, 3, 0, 0, 1000));
            monsterdrop.Add(new Item("고급 갑옷", "고급스러운 갑옷, 착용자의 몸에 딱 맞게 조절이 가능하다", ItemType.ARMOR, 0, 30, 0, 10000));
            monsterdrop.Add(new Item("고급 투구", "고급스러운 대검, 착용자의 머리에 딱 맞게 조절이 가능하다", ItemType.ARMOR, 0, 20, 0, 10000));

            monsterdrop.Add(new Item("빛나는 단검", "빛이나는 단검, 가운데에 보석이 박혀있으며 날카롭다", ItemType.WEAPON, 3, 0, 0, 10000));
            monsterdrop.Add(new Item("빛나는 대검", "빛이나는 대검, 가운데에 보석이 박혀있으며 가벼운 느낌이 든다", ItemType.WEAPON, 3, 0, 0, 1000));
            monsterdrop.Add(new Item("빛나는 갑옷", "빛이나는 갑옷, 가운데에 보석이 박혀있으며 가벼운 느낌이 든다", ItemType.ARMOR, 0, 50, 0, 100000));
            monsterdrop.Add(new Item("빛나는 투구", "빛이나는 투구, 가운데에 보석이 박혀있으며 가벼운 느낌이 든다", ItemType.ARMOR, 0, 30, 0, 100000));

            monsterdrop.Add(new Item("흐르는 그늘벼림", "다채로운 빛이 나는 단검, 착용자에게 강한 힘을 부여한다 ", ItemType.WEAPON, 3, 0, 0, 100000));
            monsterdrop.Add(new Item("흐르는 칼바람", "다채로운 빛이 나는 대검, 가벼운 무게로 착용자에게 강한 힘을 부여한다 ", ItemType.WEAPON, 3, 0, 0, 1000));
            monsterdrop.Add(new Item("지배자의 갑옷", "다채로운 빛이 나는 갑옷, 가벼운 무게로 착용자에게 강한 힘을 부여한다 ", ItemType.ARMOR, 3, 100, 0, 1000000));
            monsterdrop.Add(new Item("지배자의 투구", "다채로운 빛이 나는 투구, 가벼운 무게로 착용자에게 강한 힘을 부여한다 ", ItemType.ARMOR, 5, 80, 0, 1000000));
        }
    }
    internal class Monster
    {
        public static List<Monster> monsters = new List<Monster>();
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public bool IsAlive { get; }
        public MonsterType Type { get; }
        public Monster(string name, int level, int atk, int def, int hp, MonsterType type, bool IsAlive)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Type = type;
            this.IsAlive = true;
        }
        public static void MakeMonster()
        {//0~2 1층 , 3~5 2층, 6~8 3층, 9~11 4층
            monsters.Add(new Monster("미니언", 2, 5, 0, 15, MonsterType.small, true));
            monsters.Add(new Monster("공허충", 3, 9, 0, 10, MonsterType.small, true));
            monsters.Add(new Monster("대포미니언", 5, 8, 0, 25, MonsterType.small, true));

            monsters.Add(new Monster("붉은 덩쿨정령", 4, 10, 0, 50, MonsterType.buff, true));
            monsters.Add(new Monster("푸른 골렘", 6, 8, 0, 45, MonsterType.buff, true));
            monsters.Add(new Monster("브리아레오스", 10, 13, 0, 70, MonsterType.buff, true));

            monsters.Add(new Monster("화염 드래곤", 15, 20, 0, 100, MonsterType.epic, true));
            monsters.Add(new Monster("화학공학 드래곤", 12, 22, 0, 100, MonsterType.epic, true));
            monsters.Add(new Monster("마법공학 드래곤", 14, 18, 0, 125, MonsterType.epic, true));

            monsters.Add(new Monster("장로 드래곤", 30, 35, 0, 600, MonsterType.boss, true));
            monsters.Add(new Monster("바론 남작", 30, 40, 0, 500, MonsterType.boss, true));
            monsters.Add(new Monster("베히모스", 30, 40, 0, 600, MonsterType.boss, true));
        }
    }

}