using System;
using System.IO;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Serialization
{
    /// <summary>
    /// Serializable data of highest achieved scores
    /// </summary>
    [System.Serializable]
    public class ScoreData
    {
        /* --------------------- Constants --------------------- */
        
        public const string SAVE_PATH = "scores.dat";     // Relative path to the save file
        private const int MAX_SAVED_SCORES = 30;          // Scores worse than the 30th best are not saved
        
        private const int AUTHORS_PB = 69000;             // Mentioned so the leaderboard doesn't look empty
        private const string AUTHOR = "TIZI";             // That's me!

        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// Setters of the scores sorted by score achieved
        /// </summary>
        public string[] names = new string[MAX_SAVED_SCORES];
        
        /// <summary>
        /// Highest achieved scores sorted from highest to lowest
        /// </summary>
        public int[] sortedScores = new int[MAX_SAVED_SCORES];
        
        /// <summary>
        /// Number of saved scores
        /// </summary>
        public int savedScoresCount = 0;

        /* ------------------- Public Methods ------------------- */
        
        /// <summary>
        /// Loads data from the scores save file. Create a new save file if it doesn't exist
        /// </summary>
        /// <returns> Object with values retrieved from the scores save file </returns>
        public static ScoreData LoadScores()
        {
            try
            {
                return SaveSystem.LoadData<ScoreData>(SAVE_PATH);
            }
            catch
            {
                var newData = new ScoreData();
                newData.AddScore(AUTHORS_PB, AUTHOR);
                SaveSystem.SaveData(newData, SAVE_PATH);
                return newData;
            }
        }

        /// <summary>
        /// Checks if the score is high enough to be saved and adds it to the array of scores in the correct position.
        /// If yes the score at the lowest position will be lost!
        /// </summary>
        /// <param name="score"> Score achieved </param>
        /// <param name="name"> Setter of the score </param>
        public void AddScore(int score, string name)
        {
            int lowestScore = sortedScores[^1];
            if (score <= lowestScore) return;
            
            for (int i = sortedScores.Length - 1; i >= 0 ; i--)
            {
                int comparedScore = sortedScores[i];
                if (score <= comparedScore) break;
                
                // Push lower score down
                if (i != sortedScores.Length - 1)
                {
                    sortedScores[i + 1] = comparedScore;
                    names[i + 1] = names[i];
                }

                sortedScores[i] = score;
                names[i] = name;
            }
            
            savedScoresCount = Math.Min(savedScoresCount + 1, MAX_SAVED_SCORES);
        }

        /// <summary>
        /// Checks if the score is high enough to be saved and adds it to the array of scores in the correct position.
        /// If yes the score at the lowest position will be lost! Then the data are saved to the scores save file.
        /// </summary>
        /// <param name="score"> Achieved score </param>
        /// <param name="name"> Setter of the score </param>
        public void AddScoreAndSave(int score, string name)
        {
            AddScore(score, name);
            SaveSystem.SaveData(this, SAVE_PATH);
        }
    }
}