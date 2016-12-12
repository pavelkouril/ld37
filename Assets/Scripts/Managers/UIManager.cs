using System;
using System.Collections;
using System.Collections.Generic;
using OneRoomFactory.Transporters;
using UnityEngine;
using UnityEngine.UI;
using OneRoomFactory.Factory;
using UnityEngine.SceneManagement;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(ConstructionManager))]
    public class UIManager : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject BuildMenu;
        public GameObject HandMenu;
        public GameObject PlayGUI;
        public Text TimerText;
        public Text MoneyBalanceText;
        public GameObject TextBackground;
        public Text TextMessage;

        public Hand selectedHand;

        private bool gamePaused;
        private bool enableInputs = false;

        private ConstructionManager constructionManager;
        private MoneyManager moneyManager;

        private void Awake()
        {
            constructionManager = GetComponent<ConstructionManager>();
        }

        private void Start()
        {
            PlayGUI.SetActive(false);
            TextBackground.SetActive(false);
        }

        private void Update()
        {
            if (enableInputs)
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
            HandMenu.SetActive(false);
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

        public void EnablePlaying()
        {
            PlayGUI.SetActive(true);
            enableInputs = true;
        }

        public void QuitToMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ShowBuildMenu()
        {
            constructionManager.HideBuildingMode();
            HandMenu.SetActive(false);
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

        public void ShowHandPanel(Hand hand)
        {
            BuildMenu.SetActive(false);
            HandMenu.SetActive(true);
            selectedHand = hand;
        }

        public void SelectTargetForHand()
        {
            HandMenu.SetActive(false);
            if (selectedHand != null)
            {
                selectedHand.Tile.TileManager.ShowTiles();
                selectedHand.Tile.TileManager.SelectTargetTileForHand(selectedHand);
            }
        }

        public void ChangeHandType(int type)
        {
            HandMenu.SetActive(false);
            selectedHand.AcceptedType = (MovableType)type;
        }
    }
}