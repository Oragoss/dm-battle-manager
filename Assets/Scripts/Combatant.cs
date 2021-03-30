using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Combatant
    {
        public string Name { get; set; }
        public bool Enemy { get; set; }
        public int ArmorClass { get; set; }
        //TODO: Calculate this based on the base dice at the start of the battle
        public int Health { get; set; }
        public int AttackModifier { get; set; }
        //TODO: Make this a damage die (like 4,6,8, etc) Maybe as an enum??
        public int AttackDamage { get; set; }
        public int NumberOfAttacks { get; set; }
        public int AdditionalAttackDamage { get; set; }
    }
}
