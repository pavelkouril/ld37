using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    public class GamestateManager : MonoBehaviour
    {
        public int GameLengthInSeconds = 600;
        public bool ShowTutorial = true;
        public int RemaningTimeInSeconds { get; set; }

        private UIManager uiManager;
        private SuppliesManager suppliesManager;

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
            suppliesManager = GetComponent<SuppliesManager>();
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
            uiManager.DisplayText("Hello! Welcome! How are you?", 4);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("You have recently acquired this empty factory.", 4);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("The market is booming for custom PCBs, so you want to make them too!", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("You will need to transform supplies by <b>Belts</b> and <b>Hands</b> to Stations.", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("First you need to transfer <b>Cuprexit</b> to <b>UV Station</b>.", 5);
            yield return new WaitForSeconds(6);
            uiManager.DisplayText("Press <b>B</b> or click on the <b>Build</b> icon to open <b>Buy menu</b>.", 5);
            yield return new WaitForSeconds(6);
            uiManager.EnablePlaying();
        }

        private IEnumerator UpdateTime()
        {
            while (true)
            {
                RemaningTimeInSeconds--;
                uiManager.UpdateTimer(TimeSpan.FromSeconds(RemaningTimeInSeconds));
                yield return new WaitForSeconds(1);
            }
        }

        internal void StartShippingAcid()
        {
            suppliesManager.AcidAccepted = true;
            StartCoroutine(suppliesManager.ShipAcid());
            uiManager.DisplayText("You will now also recieve <b>Acid</b>, needed in <b>Acid Sink Station</b> to process your boards!", 5);
        }

        internal void StartShippingElectronics()
        {
            suppliesManager.ElectronicsAccepted = true;
            StartCoroutine(suppliesManager.ShipElectronics());
            uiManager.DisplayText("You will now also recieve <b>Electronics</b>, needed in <b>Assembly Station</b> to complete your PCB! ", 5);
        }

        internal void StartShippingCuprexit()
        {
            suppliesManager.CuprexitAccepted = true;
            StartCoroutine(suppliesManager.ShipCuprexit());
            uiManager.DisplayText("Be ready for your first shipment of <b>Cuprexit</b>.", 5);
        }
    }
}