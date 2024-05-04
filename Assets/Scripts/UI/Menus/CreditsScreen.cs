using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace UI.Menus
{
    public class CreditsScreen : MenuScreen
    {
        public CreditsSo credits;
        public TMP_Text creditsTemplate;
        
        public Vector2 offset = new Vector2(0, -50);
        
        public MainMenu mainMenu;
        
        void Start()
        {
            CloseMenu();
            GenerateCredits();

            creditsTemplate.gameObject.SetActive(false);
        }
        
        public void GoToMainMenu()
        {
            CloseMenu();
            mainMenu.OpenMenu();
        }
        
        private void GenerateCredits()
        {
            for (int i = 0; i < credits.credits.Count; i++)
            {
                var credit = credits.credits[i];
                var creditObject = Instantiate(creditsTemplate, transform);
                creditObject.rectTransform.anchoredPosition += offset * i;
                
                string format = creditsTemplate.text;
                creditObject.text = String.Format(format, credit.role, credit.name);
            }
        }
    }
}