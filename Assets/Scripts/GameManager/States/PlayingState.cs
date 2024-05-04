using System;
using PlayerScripts;
using UnityEngine;

namespace GameManager
{
    /// <summary>
    /// Default state of the level, when gameplay occurs
    /// </summary>
    public class PlayingState : GameState
    {
        /*------------------ Public Fields ----------------*/
        
        public PausedState pausedState;
        
        /*------------------ Public Methods ---------------*/
        
        // Unfreezes time and enables pausing
        public override void ChangeToThisState()
        {
            Time.timeScale = 1;
            
            if (Input.GetButton("Cancel")) Controls.CancelRelease += EnablePausing;
            else Controls.Cancel += Pause;
        }

        // Freezes time
        public override void LeaveThisState()
        {
            Time.timeScale = 0;
            Controls.Cancel -= Pause;
        }

        /*---------------- Private Methods ----------------*/
        
        // Enters the paused state
        private void Pause()
        {
            if (pausedState == null)
            {
                throw new NullReferenceException("pausedState not specified");
            }
         
            GameManager.Instance.ChangeState(pausedState);
        }

        // Prevents pausing immediately after unpausing
        private void EnablePausing()
        {
            Controls.CancelRelease -= EnablePausing;
            Controls.Cancel += Pause;
        }
        
        // Unsuscribe from events when the object is destroyed
        private void OnDestroy()
        {
            Controls.CancelRelease -= EnablePausing;
            Controls.Cancel -= Pause;
        }
    }
}