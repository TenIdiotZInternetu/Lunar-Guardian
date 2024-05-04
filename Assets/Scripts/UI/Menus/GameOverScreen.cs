using System;
using Serialization;
using PlayerScripts;
using TMPro;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class GameOverScreen : MenuScreen
    {
        public SceneReference mainMenuScene;

        public void Continue()
        {
            PlayerStatus.Instance.Reset();
            CloseMenu();
        }
    }
}