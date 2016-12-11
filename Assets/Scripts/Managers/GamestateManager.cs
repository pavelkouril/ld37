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

        private void Awake()
        {
            uiManager = GetComponent<UIManager>();
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
    }
}