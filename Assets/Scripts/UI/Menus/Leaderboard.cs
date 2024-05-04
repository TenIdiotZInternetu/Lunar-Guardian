using System;
using Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace UI.Menus
{
    public class Leaderboard : MenuScreen
    {
        public MainMenu mainMenu;
        public TMP_Text entryTemplate;
        
        public int maxEntryCount = 8;
        public int maxNameLength = 8;
        
        public Vector2 offset = new Vector2(10, -32);
        
        private ScoreData _scoreData;
        
        void Start()
        {
            _scoreData = ScoreData.LoadScores();
            GenerateEntries();
            CloseMenu();
            
            entryTemplate.gameObject.SetActive(false);
        }
        
        public void GoToMainMenu()
        {
            CloseMenu();
            mainMenu.OpenMenu();
        }
        
        private void GenerateEntries()
        {
            int entryCount = Mathf.Min(maxEntryCount, _scoreData.savedScoresCount);

            for (int i = 0; i < entryCount; i++)
            {
                var creditObject = Instantiate(entryTemplate, transform);
                creditObject.rectTransform.anchoredPosition += offset * i;
                
                string format = entryTemplate.text;
                
                int position = i + 1;
                string player = FixNameLength(_scoreData.names[i]);
                int score = _scoreData.sortedScores[i];
                
                creditObject.text = String.Format(format, position, player, score);
            }
        }

        private string FixNameLength(string playersName)
        {
            if (playersName.Length > maxNameLength)
            {
                playersName = playersName.Substring(0, maxNameLength);
            }
            
            if (playersName.Length < maxNameLength && playersName.Length > 0)
            {
                playersName = playersName.PadRight(maxNameLength, ' ');
            }
            
            if (playersName == "")
            {
                playersName = new String('.', maxNameLength);
            }
            
            return playersName;
        }
    }
}