using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OneRoomFactory.Managers
{
    public class HighScoresManager : MonoBehaviour
    {
        const string Filename = "hs.txt";

        public void WriteHighScore(int score)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filename, true))
            {
                file.WriteLine(DateTime.Now.ToBinary() + " " + score);
            }
        }

        public IEnumerable<KeyValuePair<DateTime, int>> GetTop10Scores()
        {
            var dict = new Dictionary<DateTime, int>();
            if (System.IO.File.Exists(Filename))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(Filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var split = line.Split(' ');
                        var date = DateTime.FromBinary(long.Parse(split[0]));
                        var score = int.Parse(split[1]);
                        dict.Add(date, score);
                    }
                }
            }
            return dict.OrderByDescending(p => p.Value).Take(10);
        }
    }
}