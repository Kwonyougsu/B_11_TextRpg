using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_B_11_RPG
{
    internal class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public Monster(string name, int level, int atk, int def, int hp)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
        }
    }
}
