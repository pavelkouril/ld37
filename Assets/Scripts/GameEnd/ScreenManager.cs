using OneRoomFactory.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OneRoomFactory.GameEnd
{
    public class ScreenManager : MonoBehaviour
    {
        public Text HighScoreTable;

        private HighScoresManager hsm;

        private void Awake()
        {
            hsm = GetComponent<HighScoresManager>();
        }

        private void Start()
        {
            var str = string.Empty;
            foreach (var p in hsm.GetTop10Scores())
            {
                str += p.Key.ToString("yyyy-MM-dd HH:mm") + "          " + p.Value + "\n";
            }
            HighScoreTable.text = str;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}