using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Battle : MonoBehaviour
    {
        #region Ui Elements

        [SerializeField]
        GameObject welcomePage;

        [SerializeField]
        Text instructions;

        [SerializeField]
        GameObject questionsPanel;

        [SerializeField]
        GameObject battlePanel;

        [SerializeField]
        GameObject addAllyCombatantButton;

        [SerializeField]
        GameObject doneWithAlliesButton;

        [SerializeField]
        GameObject addEnemyCombatantButton;

        [SerializeField]
        GameObject doneWithEnemiesButton;

        [SerializeField]
        InputField nameTextField;

        [SerializeField]
        InputField armorClassTextField;

        [SerializeField]
        InputField healthTextField;

        [SerializeField]
        InputField attackModifierTextField;

        [SerializeField]
        InputField attackDamageTextField;

        [SerializeField]
        InputField numberOfAttacksTextField;

        #endregion

        List<Combatant> allyCombatants = new List<Combatant>();
        List<Combatant> enemyCombatants = new List<Combatant>();

        private void Start()
        {
            instructions.text = "";
            questionsPanel.gameObject.SetActive(false);
            battlePanel.gameObject.SetActive(false);
            welcomePage.gameObject.SetActive(true);


            addAllyCombatantButton.gameObject.SetActive(true);
            doneWithAlliesButton.gameObject.SetActive(true);

            addEnemyCombatantButton.gameObject.SetActive(false);
            doneWithEnemiesButton.gameObject.SetActive(false);

            SampleBattle();
        }

        public void Initiate()
        {
            welcomePage.gameObject.SetActive(false);
            questionsPanel.gameObject.SetActive(true);

            instructions.text = "Enter first ally's stats";
            addAllyCombatantButton.gameObject.SetActive(true);
        }

        public void AddAllyCombatant()
        {
            var newName = nameTextField.GetComponent<InputField>().text;
            var newAC = int.Parse(armorClassTextField.GetComponent<InputField>().text);
            var newHealth = int.Parse(healthTextField.GetComponent<InputField>().text);
            var newAttackMod = int.Parse(attackModifierTextField.GetComponent<InputField>().text);
            var newDamage = int.Parse(attackDamageTextField.GetComponent<InputField>().text);
            var newNumberOfAttacks = int.Parse(numberOfAttacksTextField.GetComponent<InputField>().text);

            allyCombatants.Add(new Combatant()
            {
                Name = newName,
                Enemy = false,
                ArmorClass = newAC,
                Health = newHealth,
                AttackModifier = newAttackMod,
                AttackDamage = newDamage,
                NumberOfAttacks = newNumberOfAttacks
            });

            ClearStatFields();
        }

        public void DoneWithAllies()
        {
            ClearStatFields();

            instructions.text = "Enter first enemy's stats";
            addAllyCombatantButton.gameObject.SetActive(false);
            doneWithAlliesButton.gameObject.SetActive(false);

            addEnemyCombatantButton.gameObject.SetActive(true);
            doneWithEnemiesButton.gameObject.SetActive(true);
        }

        public void AddEnemyCombatant()
        {
            var newName = nameTextField.GetComponent<InputField>().text;
            var newAC = int.Parse(armorClassTextField.GetComponent<InputField>().text);
            var newHealth = int.Parse(healthTextField.GetComponent<InputField>().text);
            var newAttackMod = int.Parse(attackModifierTextField.GetComponent<InputField>().text);
            var newDamage = int.Parse(attackDamageTextField.GetComponent<InputField>().text);
            var newNumberOfAttacks = int.Parse(numberOfAttacksTextField.GetComponent<InputField>().text);

            enemyCombatants.Add(new Combatant()
            {
                Name = newName,
                Enemy = false,
                ArmorClass = newAC,
                Health = newHealth,
                AttackModifier = newAttackMod,
                AttackDamage = newDamage,
                NumberOfAttacks = newNumberOfAttacks
            });

            ClearStatFields();
        }

        public void DoneWithEnemies()
        {
            ClearStatFields();
            questionsPanel.gameObject.SetActive(false);

            instructions.text = "";
            battlePanel.gameObject.SetActive(true);

            CalculateBattleResults();
        }

        public void CalculateBattleResults()
        {
            var random = new System.Random(Guid.NewGuid().GetHashCode());

            for (int x = 0; x < allyCombatants.Count; x++)
            {
                if(allyCombatants[x].Health > 0)
                {
                    List<Combatant> livingPoolOfEnemies = enemyCombatants.Where(e => e.Health > 0).ToList();

                    var attackRoll = random.Next(0, 19) + allyCombatants[x].AttackModifier;
                    var randomEnemy = enemyCombatants[random.Next(0, enemyCombatants.Count)];
                    

                    if (attackRoll >= randomEnemy.ArmorClass)
                    {
                        randomEnemy.Health -= allyCombatants[x].AttackDamage;
                        if (randomEnemy.Health <= 0)
                            Debug.Log($"{randomEnemy.Name} has been slain!");
                        else
                            Debug.Log($"{randomEnemy.Name} took {allyCombatants[x].AttackDamage} damaage from {allyCombatants[x].Name}! He now has {randomEnemy.Health} health.");
                    }
                    else
                        Debug.Log($"{randomEnemy.Name} was not hit by {allyCombatants[x].Name}");
                }
                else
                    Debug.Log($"{allyCombatants[x].Name} is slain...");
            }

            for (int y = 0; y < enemyCombatants.Count; y++)
            {
                if(enemyCombatants[y].Health > 0)
                {
                    var attackRoll = random.Next(0, 19) + enemyCombatants[y].AttackModifier;
                    var randomAlly = allyCombatants[random.Next(0, allyCombatants.Count)];

                    if (attackRoll >= randomAlly.ArmorClass)
                    {
                        randomAlly.Health -= enemyCombatants[y].AttackDamage;
                        if (randomAlly.Health <= 0)
                            Debug.Log($"{randomAlly.Name} has been slain!");
                        else
                            Debug.Log($"{randomAlly.Name} took {enemyCombatants[y].AttackDamage} damaage from {enemyCombatants[y].Name}! He now has {randomAlly.Health} health.");
                    }
                    else
                        Debug.Log($"{randomAlly.Name} was not hit by {enemyCombatants[y].Name}");
                }
                else
                    Debug.Log($"{enemyCombatants[y].Name} is slain...");
            }

            //TODO: Add these numbers and texts to the UI
        }

        void ClearStatFields()
        {
            nameTextField.GetComponent<InputField>().text = "";
            armorClassTextField.GetComponent<InputField>().text = "";
            healthTextField.GetComponent<InputField>().text = "";
            attackModifierTextField.GetComponent<InputField>().text = "";
            attackDamageTextField.GetComponent<InputField>().text = "";
            numberOfAttacksTextField.GetComponent<InputField>().text = "";
        }

        void SampleBattle()
        {
            Debug.Log("Kicking off a simple example battle...");

            var ungor = new Combatant()
            {
                Name = "Ungor",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            };

            var gor = new Combatant()
            {
                Name = "Gor",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 1
            };

            var bestigor = new Combatant()
            {
                Name = "Bestigor",
                Enemy = false,
                ArmorClass = 16,
                Health = 93, //11d8+44
                AttackModifier = 6,
                AttackDamage = 15,
                NumberOfAttacks = 1
            };

            var commoner = new Combatant()
            {
                Name = "Commoner",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            };

            var guard = new Combatant()
            {
                Name = "Guard",
                Enemy = false,
                ArmorClass = 16,
                Health = 11, //2d8+2
                AttackModifier = 3,
                AttackDamage = 4,
                NumberOfAttacks = 1
            };

            var dretch = new Combatant()
            {
                Name = "Dretch",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            };

            var maw = new Combatant()
            {
                Name = "Maw",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            };

            var hezrou = new Combatant()
            {
                Name = "Hezrou",
                Enemy = true,
                ArmorClass = 63,
                Health = 136, //13d10+65
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 3
            };

            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);
            allyCombatants.Add(ungor);

            allyCombatants.Add(gor);
            allyCombatants.Add(gor);
            allyCombatants.Add(gor);
            allyCombatants.Add(gor);
            allyCombatants.Add(gor);
            allyCombatants.Add(gor);

            allyCombatants.Add(bestigor);
            allyCombatants.Add(bestigor);
            allyCombatants.Add(bestigor);

            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);
            enemyCombatants.Add(dretch);

            enemyCombatants.Add(maw);
            enemyCombatants.Add(maw);
            enemyCombatants.Add(maw);
            enemyCombatants.Add(maw);

            enemyCombatants.Add(hezrou);
            enemyCombatants.Add(hezrou);
        }
    }
}
