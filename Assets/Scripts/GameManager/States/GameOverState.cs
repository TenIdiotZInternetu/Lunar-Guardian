using PlayerScripts;
using UI.Menus;

namespace GameManager
{
    /// <summary>
    /// State reached upon losing all health
    /// </summary>
    public class GameOverState : GameState
    {
        /*------------------ Public Fields ------------------*/
        
        public GameOverScreen gameOverScreen;
        
        /*----------------- Unity Callbacks -----------------*/
        
        void Start()
        {
            PlayerStatus.GameOverEvent += OnGameOver;
        }
        
        /*------------------ Public Methods -----------------*/

        public override void ChangeToThisState()
        {
            gameOverScreen.OpenMenu();
        }

        public override void LeaveThisState()
        {
            
        }
        
        /*----------------- Private Methods -----------------*/
        
        // Called when the player dies
        private void OnGameOver()
        {
            GameManager.Instance.ChangeState(this);
        }

        // Unsubscribe from the event when the object is destroyed
        private void OnDestroy()
        {
            PlayerStatus.GameOverEvent -= OnGameOver;
        }
    }
}