using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tymski;
using UnityEngine.Events;

namespace UI.Menus
{
    public class MainMenu : MenuScreen
    {
        public SceneReference level1;
        public CreditsScreen creditsScreen;
        public Leaderboard leaderboard;

        public void Start()
        {
            Time.timeScale = 1;
            OpenMenu();
        }
        
        public void StartGame()
        {
            SceneManager.LoadScene(level1);
        }
        
        public void GoToCredits()
        {
            CloseMenu();
            creditsScreen.OpenMenu();
        }
        
        public void GoToLeaderboard()
        {
            CloseMenu();
            leaderboard.OpenMenu();
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}