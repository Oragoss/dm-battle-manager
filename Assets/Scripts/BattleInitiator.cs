using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class BattleInitiator
    {
        public static void SampleBattle()
        {
            Debug.Log("Kicking off a simple example battle...");

            var ungor = new Combatant()
            {
                Name = "Ungor",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9
            };

            var gor = new Combatant()
            {
                Name = "Gor",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7
            };

            //var bestigor = new Combatant()
            //{
            //    Name = "Bestigor",
            //    Enemy = false,
            //    ArmorClass = 16,
            //    Health = 93, //11d8+44
            //    AttackModifier = 6,
            //    AttackDamage = 15
            //};

            var commoner = new Combatant()
            {
                Name = "Commoner",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2
            };

            var guard = new Combatant()
            {
                Name = "Guard",
                Enemy = false,
                ArmorClass = 16,
                Health = 11, //2d8+2
                AttackModifier = 3,
                AttackDamage = 4
            };

            var dretch = new Combatant()
            {
                Name = "Dretch",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4
            };

            var maw = new Combatant()
            {
                Name = "Maw",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8
            };

            //var hezrou = new Combatant()
            //{
            //    Name = "Hezrou",
            //    Enemy = true,
            //    ArmorClass = 63,
            //    Health = 136, //13d10+65
            //    AttackModifier = 4,
            //    AttackDamage = 8
            //    NumberOfAttacks = 3
            //};
        }
    }
}
