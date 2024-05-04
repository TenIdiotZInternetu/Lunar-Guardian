using System;
using PlayerScripts;
using UnityEngine;

namespace Spawnables.Pickups
{
    /// <summary>
    /// Grabbing this pickup increases the score multiplier. The score multiplier is reset by
    /// taking damage or by letting the pickup leave the playfield.
    /// </summary>
    public class MultiplierPickup : Pickup
    {
        /* ------------------- Unity Callbacks ------------------- */
        
        // The multiplier resets when the pickup leaves the playfield
        protected void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayfieldBorder")) return;
            PlayerStatus.Instance.ResetScoreMultiplier();
        }
    }
}