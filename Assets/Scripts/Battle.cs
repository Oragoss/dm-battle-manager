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

        [Header("Battle Result Variables")]
        [Tooltip("How far away you want a unit label to be from the edge or center on the x axis.")]
        [SerializeField]
        float distanceFromTheEdge = 150;

        [Tooltip("How far away you want a unit label to be from each other on the y axis.")]
        [SerializeField]
        float distanceFromOtherListItems = 25;

        [Tooltip("Where do you want the unit labels to start on the y axis.")]
        [SerializeField]
        float startingPosition = 120;

        [Header("UI Elements")]
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

        [SerializeField]
        Text unitResultLabelTextPrefab;
        [SerializeField]
        Text unitResultTextPrefab;

        [SerializeField]
        GameObject allyConent;
        [SerializeField]
        GameObject enemyContent;

        [SerializeField]
        Text roundTitle;

        #endregion

        List<Combatant> allyCombatants = new List<Combatant>();
        List<Combatant> enemyCombatants = new List<Combatant>();

        int roundNumber = 1;

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

            //TODO: Make this a button
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
            //TODO: Add a way to add multiple allies of the same type
            var newName = nameTextField.GetComponent<InputField>().text;
            var newAC = int.Parse(armorClassTextField.GetComponent<InputField>().text);
            var newHealth = int.Parse(healthTextField.GetComponent<InputField>().text);
            var newAttackMod = int.Parse(attackModifierTextField.GetComponent<InputField>().text);
            var newDamage = int.Parse(attackDamageTextField.GetComponent<InputField>().text);
            var newNumberOfAttacks = int.Parse(numberOfAttacksTextField.GetComponent<InputField>().text);

            allyCombatants.Add(new Combatant()
            {
                Name = $"{newName}_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
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
            //TODO: Add a way to add multiple enemies of the same type
            var newName = nameTextField.GetComponent<InputField>().text;
            var newAC = int.Parse(armorClassTextField.GetComponent<InputField>().text);
            var newHealth = int.Parse(healthTextField.GetComponent<InputField>().text);
            var newAttackMod = int.Parse(attackModifierTextField.GetComponent<InputField>().text);
            var newDamage = int.Parse(attackDamageTextField.GetComponent<InputField>().text);
            var newNumberOfAttacks = int.Parse(numberOfAttacksTextField.GetComponent<InputField>().text);

            enemyCombatants.Add(new Combatant()
            {
                Name = $"{newName}_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
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
            var random = new System.Random(Mathf.Abs(Guid.NewGuid().GetHashCode()));
            Debug.Log($"Round {roundNumber}: FIGHT!");
            roundTitle.text = $"Round {roundNumber}: FIGHT!";
            roundNumber++;
            ClearBattleResults();
            try
            {
                for (int x = 0; x < allyCombatants.Count; x++)
                {
                    if (allyCombatants[x].Health > 0)
                    {
                        List<Combatant> livingPoolOfEnemies = enemyCombatants.Where(e => e.Health > 0).ToList();
                        if (livingPoolOfEnemies != null && livingPoolOfEnemies.Count <= 0)
                        {
                            Debug.Log($"{allyCombatants[x].Name} stands victorious with no more enemies to fight.");
                            InstantiateAllyBattleResultUiElements(allyCombatants[x].Name, "stands victorious with no more enemies to fight.", x);
                            continue;
                        }
                        var randomEnemy = livingPoolOfEnemies[random.Next(0, livingPoolOfEnemies.Count)];

                        for (int i = 0; i < allyCombatants[x].NumberOfAttacks; i++)
                        {
                            var attackRoll = random.Next(0, 19) + allyCombatants[x].AttackModifier;

                            if (attackRoll >= randomEnemy.ArmorClass)
                            {
                                randomEnemy.Health -= allyCombatants[x].AttackDamage;
                                if (randomEnemy.Health <= 0)
                                {
                                    Debug.Log($"<color=yellow>{randomEnemy.Name} has been slain!</color>");
                                    var multiAttack = allyCombatants[x].NumberOfAttacks - 1 <= 0 ? 1 : allyCombatants[x].NumberOfAttacks;
                                    if (i < multiAttack)
                                    {
                                        //Target a new living enemy
                                        Debug.Log($"{allyCombatants[x].Name} has attacked {i + 1} times");
                                        livingPoolOfEnemies = enemyCombatants.Where(e => e.Health > 0).ToList();
                                        if (livingPoolOfEnemies != null && livingPoolOfEnemies.Count > 0)
                                            randomEnemy = livingPoolOfEnemies[random.Next(0, livingPoolOfEnemies.Count)];
                                        else
                                        {
                                            InstantiateAllyBattleResultUiElements(allyCombatants[x].Name, "stands victorious with no more enemies to fight.", x);
                                            Debug.Log($"{allyCombatants[x].Name} stands victorious with no more enemies to fight.");
                                            continue;
                                        }
                                    }
                                }
                                else
                                    Debug.Log($"{randomEnemy.Name} took {allyCombatants[x].AttackDamage} damage from {allyCombatants[x].Name}! He now has {randomEnemy.Health} health.");
                            }
                            else
                                Debug.Log($"{randomEnemy.Name} was not hit by {allyCombatants[x].Name}");
                        }
                        InstantiateAllyBattleResultUiElements(allyCombatants[x].Name, allyCombatants[x].Health.ToString(), x);
                    }
                    else
                    {
                        InstantiateAllyBattleResultUiElements(allyCombatants[x].Name, "X", x);
                        Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX{allyCombatants[x].Name} is dead and did nothing...");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error calculating allies' actions: " + ex.Message);
            }

            try
            {
                for (int y = 0; y < enemyCombatants.Count; y++)
                {
                    if (enemyCombatants[y].Health > 0)
                    {
                        List<Combatant> livingPoolOfAllies = allyCombatants.Where(e => e.Health > 0).ToList();
                        if (livingPoolOfAllies != null && livingPoolOfAllies.Count <= 0)
                        {
                            Debug.Log($"{enemyCombatants[y].Name} stands victorious with no more enemies to fight.");
                            InstantiateAllyBattleResultUiElements(enemyCombatants[y].Name, "stands victorious with no more enemies to fight.", y);
                            continue;
                        }
                        var randomAlly = livingPoolOfAllies[random.Next(0, livingPoolOfAllies.Count)];

                        for (int i = 0; i < enemyCombatants[y].NumberOfAttacks; i++)
                        {
                            var attackRoll = random.Next(0, 19) + enemyCombatants[y].AttackModifier;

                            if (attackRoll >= randomAlly.ArmorClass)
                            {
                                randomAlly.Health -= enemyCombatants[y].AttackDamage;
                                if (randomAlly.Health <= 0)
                                {
                                    Debug.Log($"<color=yellow>{randomAlly.Name} has been slain!</color>");
                                    var multiAttack = enemyCombatants[y].NumberOfAttacks - 1 <= 0 ? 1 : enemyCombatants[y].NumberOfAttacks;
                                    if (i < multiAttack)
                                    {
                                        //Target a new living enemy
                                        Debug.Log($"{enemyCombatants[y].Name} has attacked {i + 1} times");
                                        livingPoolOfAllies = allyCombatants.Where(e => e.Health > 0).ToList();
                                        if (livingPoolOfAllies != null && livingPoolOfAllies.Count > 0)
                                            randomAlly = livingPoolOfAllies[random.Next(0, livingPoolOfAllies.Count)];
                                        else
                                        {
                                            Debug.Log($"{enemyCombatants[y].Name} stands victorious with no more enemies to fight.");
                                            continue;
                                        }
                                    }
                                }
                                else
                                    Debug.Log($"{randomAlly.Name} took {enemyCombatants[y].AttackDamage} damage from {enemyCombatants[y].Name}! He now has {randomAlly.Health} health.");
                            }
                            else
                                Debug.Log($"{randomAlly.Name} was not hit by {enemyCombatants[y].Name}");
                        }
                        InstantiateEnemyBattleResultUiElements(enemyCombatants[y].Name, enemyCombatants[y].Health.ToString(), y);
                    }
                    else
                    {
                        InstantiateEnemyBattleResultUiElements(enemyCombatants[y].Name, "X", y);
                        Debug.Log($"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX{enemyCombatants[y].Name} is dead and did nothing...");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error calculating allies' actions: " + ex.Message);
            }
        }

        public void InstantiateAllyBattleResultUiElements(string name, string message, int itemNumber)
        {
            //TODO: Set a dynamic anchor for the text fields
            Text allyUnitResultLabelTextField = Instantiate(unitResultLabelTextPrefab, new Vector3(transform.position.x - 300, -((transform.position.y - startingPosition) + distanceFromOtherListItems  * itemNumber), transform.position.z), transform.rotation);
            allyUnitResultLabelTextField.transform.SetParent(allyConent.transform, false);
            allyUnitResultLabelTextField.text = name;

            Text allyUnitResultTextField = Instantiate(unitResultTextPrefab, new Vector3(transform.position.x + 150, -((transform.position.y - startingPosition) + distanceFromOtherListItems  * itemNumber), transform.position.z), transform.rotation);
            allyUnitResultTextField.transform.SetParent(allyConent.transform, false);
            allyUnitResultTextField.text = message;       
        }

        public void InstantiateEnemyBattleResultUiElements(string name, string message, int itemNumber)
        {
            //TODO: Set a dynamic anchor for the text fields
            Text enemyUnitResultLabelTextField = Instantiate(unitResultLabelTextPrefab, new Vector3(transform.position.x - 300, -((transform.position.y - startingPosition) + distanceFromOtherListItems  * itemNumber), transform.position.z), transform.rotation);
            enemyUnitResultLabelTextField.transform.SetParent(enemyContent.transform, false);
            enemyUnitResultLabelTextField.text = name;

            Text enemyUnitResultTextField = Instantiate(unitResultTextPrefab, new Vector3(transform.position.x + 150, -((transform.position.y - startingPosition) + distanceFromOtherListItems  * itemNumber), transform.position.z), transform.rotation);
            enemyUnitResultTextField.transform.SetParent(enemyContent.transform, false);
            enemyUnitResultTextField.text = message;
        }

        public void ClearBattleResults()
        {
            var allyChildren = new List<GameObject>();
            foreach (Transform child in allyConent.transform) allyChildren.Add(child.gameObject);
            allyChildren.ForEach(child => Destroy(child));

            var enemyChildren = new List<GameObject>();
            foreach (Transform child in enemyContent.transform) allyChildren.Add(child.gameObject);
            allyChildren.ForEach(child => Destroy(child));

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

            #region Ungor
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Ungor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 15, //2d8+6
                AttackModifier = 5,
                AttackDamage = 9,
                NumberOfAttacks = 1
            });
            #endregion

            #region Gor
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 2
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 2
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 2
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Gor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 13,
                Health = 42, //5d8+20
                AttackModifier = 6,
                AttackDamage = 7,
                NumberOfAttacks = 2
            });
            #endregion

            #region Bestigor
            allyCombatants.Add(new Combatant()
            {
                Name = $"Bestigor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 16,
                Health = 93, //11d8+44
                AttackModifier = 6,
                AttackDamage = 15,
                NumberOfAttacks = 2
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Bestigor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 16,
                Health = 93, //11d8+44
                AttackModifier = 6,
                AttackDamage = 15,
                NumberOfAttacks = 2
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Bestigor_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = false,
                ArmorClass = 16,
                Health = 93, //11d8+44
                AttackModifier = 6,
                AttackDamage = 15,
                NumberOfAttacks = 2
            });
            #endregion

            #region Guard
            allyCombatants.Add(new Combatant()
            {
                Name = $"Guard_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 16,
                Health = 11, //2d8+2
                AttackModifier = 3,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Guard_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 16,
                Health = 11, //2d8+2
                AttackModifier = 3,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Guard_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 16,
                Health = 11, //2d8+2
                AttackModifier = 3,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            #endregion

            #region Commoner
            allyCombatants.Add(new Combatant()
            {
                Name = $"Commoner_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Commoner_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Commoner_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Commoner_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            });
            allyCombatants.Add(new Combatant()
            {
                Name = $"Commoner_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = false,
                ArmorClass = 10,
                Health = 4, //1d4
                AttackModifier = 2,
                AttackDamage = 2,
                NumberOfAttacks = 1
            });
            #endregion

            #region Dretch
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Mathf.Abs(Guid.NewGuid().GetHashCode()))}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Dretch_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 11,
                Health = 18, //4d6+4
                AttackModifier = 2,
                AttackDamage = 4,
                NumberOfAttacks = 1
            });
            #endregion

            #region Maw
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Maw_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Maw_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Maw_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Maw_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            });
            enemyCombatants.Add(new Combatant()
            {
                Name = $"Maw_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
                Enemy = true,
                ArmorClass = 13,
                Health = 33, //6d+6
                AttackModifier = 4,
                AttackDamage = 8,
                NumberOfAttacks = 1
            });
            #endregion

            #region Hezrou
            //enemyCombatants.Add(new Combatant()
            //{
            //    Name = $"Hezrou_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
            //    Enemy = true,
            //    ArmorClass = 63,
            //    Health = 93, //13d10+65
            //    AttackModifier = 4,
            //    AttackDamage = 8,
            //    NumberOfAttacks = 3
            //});
            //enemyCombatants.Add(new Combatant()
            //{
            //    Name = $"Hezrou_{Mathf.Abs(Guid.NewGuid().GetHashCode())}",
            //    Enemy = true,
            //    ArmorClass = 63,
            //    Health = 136, //13d10+65
            //    AttackModifier = 4,
            //    AttackDamage = 8,
            //    NumberOfAttacks = 3
            //});
            #endregion
        }
    }
}