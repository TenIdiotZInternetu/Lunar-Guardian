using System;
using Serialization;
using PlayerScripts;
using Tools;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    /// <summary>
    /// A state machine that controls the flow of the gameplay
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /*----------------- Static Fields -----------------*/
        
        /// <summary>
        /// Singleton instance of the GameManager
        /// </summary>
        public static GameManager Instance;
        
        /*----------------- Public Fields -----------------*/
        
        /// <summary>
        /// State to set upon loading a scene
        /// </summary>
        public GameState initialState;
        
        /// <summary>
        /// A scene to return to upon quitting the gameplay 
        /// </summary>
        public SceneReference mainMenuScene;

        /*--------------------- Events ---------------------*/
        
        /// <summary>
        /// Event with GameState parameter that is invoked when the state of the game changes
        /// </summary>
        public GameStateEvent onStateChanged;
        
        /*----------------- Private Fields -----------------*/
        
        private GameState _currentState;

        /*------------------ Unity Callbacks ---------------*/
        
        public void Awake()
        {
            Instance = this;

            _currentState = initialState;
            _currentState.ChangeToThisState();
            onStateChanged?.Invoke(_currentState);
        }

        /*----------------- Public Methods -----------------*/
        
        /// <summary>
        /// Activates the given state and deactivates the current state
        /// </summary>
        /// <param name="newState"> The newly activated state </param>
        public void ChangeState(GameState newState)
        {
            _currentState.LeaveThisState();
            _currentState = newState;
            _currentState.ChangeToThisState();
            
            Instance.onStateChanged?.Invoke(_currentState);
        }
        
        /// <summary>
        /// Saves the obtained score and returns to the main menu
        /// </summary>
        /// <param name="playersName"> Name of the player to save to the leaderboards </param>
        public void SaveAndQuit(string playersName)
        {
            var scoreData = ScoreData.LoadScores();
            scoreData.AddScoreAndSave(PlayerStatus.Instance.Score, playersName);
            
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}