﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneRoomFactory.Managers
{
    public class GamestateManager : MonoBehaviour
    {
        public int GameLengthInSeconds = 600;
        public bool ShowTutorial = true;
        public int RemaningTimeInSeconds { get; set; }

        private UIManager uiManager;
        private SuppliesManager suppliesManager;
        private HighScoresManager hsManager;
        private MoneyManager moneyManager;

        private bool hasHand;

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
            suppliesManager = GetComponent<SuppliesManager>();
            hsManager = GetComponent<HighScoresManager>();
            moneyManager = GetComponent<MoneyManager>();
        }

        private void Start()
        {
            RemaningTimeInSeconds = GameLengthInSeconds;
            uiManager.UpdateTimer(TimeSpan.FromSeconds(RemaningTimeInSeconds));
            StartCoroutine(UpdateTime());
            if (ShowTutorial)
            {
                StartCoroutine(ShowBasicInfo());
            }
            else
            {
                uiManager.EnablePlaying();
            }
        }

        private IEnumerator ShowBasicInfo()
        {
            yield return new WaitForSeconds(2);
            uiManager.DisplayText("Hello! Welcome! How are you? You have recently acquired this empty factory.", 4);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("The market is booming for custom PCBs, so you want to make them too!", 4);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("You will need to transform supplies by <b>Belts</b> and <b>Hands</b> to Stations.", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("First you need to transfer <b>Cuprexit</b> to <b>UV Station</b>.", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("Each Station need intermediate product from previous Station (and sometimes other supplies). Stations are numbered from 01 to 05.", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("Press <b>B</b> or click on the <b>Build</b> icon to open <b>Build menu</b> and play!", 5);
            yield return new WaitForSeconds(6);
            uiManager.EnablePlaying();
        }

        private IEnumerator UpdateTime()
        {
            while (true)
            {
                RemaningTimeInSeconds--;
                uiManager.UpdateTimer(TimeSpan.FromSeconds(RemaningTimeInSeconds));
                if (RemaningTimeInSeconds == 10)
                {
                    uiManager.DisplayText("Your game time is almost over - do you feel like you can make more money next time? Try it!", 10);
                }
                if (RemaningTimeInSeconds == 0)
                {
                    hsManager.WriteHighScore(moneyManager.Balance);
                    SceneManager.LoadScene(2);
                    yield break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        internal void StartShippingAcid()
        {
            if (!suppliesManager.AcidAccepted)
            {
                suppliesManager.AcidAccepted = true;
                StartCoroutine(suppliesManager.ShipAcid());
                uiManager.DisplayText("You will now also recieve <b>Acid</b>, needed in <b>Acid Sink Station</b> to process your boards!", 5);
            }
        }

        internal void StartShippingElectronics()
        {
            if (!suppliesManager.ElectronicsAccepted)
            {
                suppliesManager.ElectronicsAccepted = true;
                StartCoroutine(suppliesManager.ShipElectronics());
                uiManager.DisplayText("You will now also recieve <b>Electronics</b>, needed in <b>Assembly Station</b> to complete your PCB! ", 5);
            }
        }

        internal void StartShippingCuprexit()
        {
            if (!suppliesManager.CuprexitAccepted)
            {
                suppliesManager.CuprexitAccepted = true;
                StartCoroutine(suppliesManager.ShipCuprexit());
                uiManager.DisplayText("Be ready for your first shipment of <b>Cuprexit</b>.", 5);
            }
        }

        internal void ShowHandTutorial()
        {
            if (!hasHand)
            {
                hasHand = true;
                uiManager.DisplayText("You need to configure your Hand. Click on it to select <b>Target tile</b> and what <b>supply type it moves</b>!", 6);
            }
        }
    }
}