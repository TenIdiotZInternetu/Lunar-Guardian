using UnityEngine;

namespace GameManager
{
    /// <summary>
    /// A state governed by the GameManager
    /// </summary>
    public abstract class GameState : MonoBehaviour
    {
        /*------------------ Public Methods ---------------*/
        
        /// <summary>
        /// Takes actions needed to define this state
        /// </summary>
        public abstract void ChangeToThisState();
        
        /// <summary>
        /// Undoes changes made by this state
        /// </summary>
        public abstract void LeaveThisState();
    }
}