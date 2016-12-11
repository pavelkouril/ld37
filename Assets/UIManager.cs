using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    [RequireComponent(typeof(ConstructionManager))]
    public class UIManager : MonoBehaviour
    {
        public GameObject PauseMenu;
        public GameObject BuildMenu;

        private ConstructionManager constructionManager;

        private bool gamePaused;

        private void Awake()
        {
            constructionManager = GetComponent<ConstructionManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && !gamePaused)
            {
                BuildMenu.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gamePaused)
                {
                    BuildMenu.SetActive(false);
                    constructionManager.HideBuildingMode();
                    PauseMenu.SetActive(true);
                    Time.timeScale = 0;
                    gamePaused = true;
                }
                else
                {
                    ResumeGame();
                }
            }
        }

        public void ResumeGame()
        {
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

        internal void HideBuildMenu()
        {
            BuildMenu.SetActive(false);
        }
    }
}