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
        [SerializeField]
        Text mainText;
        [SerializeField]
        Text welcomeText;
        [SerializeField]
        GameObject startButton;


        #region Stats
        int numberOfCombatants;

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
            BattleInitiator.SampleBattle();
        }


        public void Initiate()
        {
            welcomeText.text = "";
            startButton.gameObject.SetActive(false);

            //TODO: Start the simulator
            mainText.text = "";
        }
    }
}
