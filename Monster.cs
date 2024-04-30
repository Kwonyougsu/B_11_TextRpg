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
        boss = 2
    }
    internal class RandomMonster
    {
        public static List<RandomMonster> randmonsters = new List<RandomMonster>();
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
        public void MonsterBattle(bool withNumber = false, int idx = 0)
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
        public static void MakeMonster(bool withNumber = false, int idx = 0)
        {
            monsters.Add(new Monster("미니언", 2, 5, 0, 15, MonsterType.small, true));
            monsters.Add(new Monster("공허충", 3, 9, 0, 10, MonsterType.small, true));
            monsters.Add(new Monster("대포미니언", 5, 8, 0, 25, MonsterType.small, true));

        }
    }
}