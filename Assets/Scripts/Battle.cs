using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region Stats

        List<Combatant> allyCombatants = new List<Combatant>();
        List<Combatant> enemyCombatants = new List<Combatant>();

        //TODO: Generalize these
        int numberOfUngor = 0;
        int ungorAttackModifier = 5;
        int ungorAttackDamage = 6;

        int numberOfGor = 0;
        int gorAttackModifier = 6;
        int gorAttackDamage = 7;

        int numberOfBestigor = 0;
        int bestigorAttackModifier = 6;
        int bestigorAttackDamage = 15;

        int numberOfGuards = 0;
        int guardAttackModifier = 4;
        int guardAttackDamage = 5;

        int numberOfCommoners = 0;
        int commonerAttackModifier = 4;
        int commonerAttackDamage = 5;

        int numberOfDretches = 0;
        int dretchAttackModifier = 2;
        int dretchAttackDamage = 3;

        int numberOfMaws = 0;
        int mawAttackModifier = 4;
        int mawAttackDamage = 11;

        int numberOfHezrou = 0;
        int hezrouAttackModifier = 7;
        int hezrouAttackDamage = 15;

        #endregion

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

            //TODO: Get a sample working
            //BattleInitiator.SampleBattle();
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

            allyCombatants.Add(new Combatant() {
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
            //todo: eek

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
    }
}
