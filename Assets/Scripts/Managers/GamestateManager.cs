using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    public class GamestateManager : MonoBehaviour
    {
        public int GameLengthInSeconds = 600;
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
            uiManager.DisplayText("You will now also recieve Acid, needed in Acid Sink station to process your boards!");
        }

        internal void StartShippingElectronics()
        {
            suppliesManager.ElectronicsAccepted = true;
            uiManager.DisplayText("You will now also recieve Electronics, needed in Assembly station to complete your PCB!");
        }
    }
}