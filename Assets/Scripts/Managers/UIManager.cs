using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(ConstructionManager))]
    public class UIManager : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject BuildMenu;
        public GameObject PlayGUI;
        public Text TimerText;
        public Text MoneyBalanceText;
        public GameObject TextBackground;
        public Text TextMessage;

        private bool gamePaused;

        private ConstructionManager constructionManager;
        private MoneyManager moneyManager;

        private void Awake()
        {
            constructionManager = GetComponent<ConstructionManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && !gamePaused)
            {
                ShowBuildMenu();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gamePaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }

        public void UpdateBalance(int money)
        {
            MoneyBalanceText.text = "$ " + money;
        }

        public void UpdateTimer(TimeSpan time)
        {
            TimerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }

        public void PauseGame()
        {
            PlayGUI.SetActive(false);
            BuildMenu.SetActive(false);
            constructionManager.HideBuildingMode();
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            gamePaused = true;
        }

        public void ResumeGame()
        {
            PlayGUI.SetActive(true);
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
            gamePaused = false;
        }

        public void QuitToMenu()
        {
        }

        public void QuitToDesktop()
        {

        }

        public void ShowBuildMenu()
        {
            constructionManager.HideBuildingMode();
            BuildMenu.SetActive(true);
        }

        internal void HideBuildMenu()
        {
            BuildMenu.SetActive(false);
        }

        public void DisplayText(string text, int duration)
        {
            TextBackground.SetActive(true);
            TextMessage.text = text;
            StartCoroutine(HideTextAfter(duration));
        }

        private IEnumerator HideTextAfter(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            TextBackground.SetActive(false);
        }
    }
}