using System;
using System.Collections.Generic;
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
    internal class Monster
    {
        public static List<Monster> monsters = new List<Monster>();
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public bool IsAlive {  get; }
        public MonsterType Type { get; }
        public Monster(string name, int level, int atk, int def, int hp, MonsterType type)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Type = type;
        }
        public static void MakeMonster()
        {
            monsters.Add(new Monster("미니언", 2, 5, 0, 15, MonsterType.small));
            monsters.Add(new Monster("공허충", 3, 9, 0, 10, MonsterType.small));
            monsters.Add(new Monster("대포미니언", 5, 8, 0, 25, MonsterType.small));

        }
        public void MonsterBattle()
        {

        }
    }
}
