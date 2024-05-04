using Serialization;
using PlayerScripts;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class PauseMenu : MenuScreen
    {
        public SceneReference mainMenuScene;
        
        public void ResumeGame()
        {
            CloseMenu();
        }
    }
}