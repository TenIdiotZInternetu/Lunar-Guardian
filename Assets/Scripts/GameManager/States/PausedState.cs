using System;
using PlayerScripts;
using Spawnables;
using UI.Menus;

namespace GameManager
{
    /// <summary>
    /// State reached pressing the pause button
    /// </summary>
    public class PausedState : GameState
    {
        /*------------------ Public Fields ------------------*/
        
        /// <summary>
        /// PlayingState to return to when unpausing
        /// </summary>
        public PlayingState playingState;
        
        /// <summary>
        /// Menu to open when pausing
        /// </summary>
        public PauseMenu pauseMenu;
        
        /*------------------ Public Methods -----------------*/
        
        public override void ChangeToThisState()
        {
            pauseMenu.OpenMenu();
            Controls.CancelRelease += EnableUnpausing;
        }

        public override void LeaveThisState()
        {
            // Prevent unpausing immediately after pausing
            Controls.Cancel -= Unpause;
        }

        /*----------------- Private Methods -----------------*/
        
        // Closes the menu and returns to the playing state
        private void Unpause()
        {
            pauseMenu.CloseMenu();
            
            if (playingState == null)
            {
                throw new NullReferenceException("playingState not specified");
            }
            
            GameManager.Instance.ChangeState(playingState);
        }

        // Prevents unpausing immediately after pausing
        private void EnableUnpausing()
        {
            Controls.CancelRelease -= EnableUnpausing;
            Controls.Cancel += Unpause;
        }
        
        // Unsubscribe from the event when the object is destroyed
        private void OnDestroy()
        {
            Controls.CancelRelease -= EnableUnpausing;
            Controls.Cancel -= Unpause;
        }
    }
}