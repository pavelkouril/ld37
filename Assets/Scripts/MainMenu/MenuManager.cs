using OneRoomFactory.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OneRoomFactory.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject MainPanel;
        public GameObject CreditsPanel;
        public GameObject HighScorePanel;
        public Text HighScoreTable;

        private HighScoresManager hsm;

        private void Awake()
        {
            hsm = GetComponent<HighScoresManager>();
        }

        private void Start()
        {
            BackToMenu();

            var str = string.Empty;
            foreach (var p in hsm.GetTop10Scores())
            {
                str += p.Key.ToString("yyyy-MM-dd HH:mm") + "          " + p.Value + "\n";
            }
            HighScoreTable.text = str;
        }
        
        public void StartGame()
        {
            MainPanel.SetActive(false);
            SceneManager.LoadScene(1);
        }

        public void BackToMenu()
        {
            MainPanel.SetActive(true);
            CreditsPanel.SetActive(false);
            HighScorePanel.SetActive(false);
        }

        public void ShowHighScore()
        {
            MainPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            HighScorePanel.SetActive(true);
        }

        public void ShowCredits()
        {
            MainPanel.SetActive(false);
            CreditsPanel.SetActive(true);
            HighScorePanel.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
