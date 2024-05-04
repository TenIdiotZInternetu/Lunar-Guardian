using System;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

namespace Spawnables.Weapons
{
    /// <summary>
    /// An object responsible for timing and spawning projectiles or other IShootables
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// True if the weapon should shooting in the current frame
        /// </summary>
        public bool HasAggro { get; set; } = false;
        
        /// <summary>
        /// Time between shots in seconds
        /// </summary>
        public float cooldown;

        /// <summary>
        /// Time between releasing larger set of shots
        /// </summary>
        public float chargeTime;
        
        /// <summary>
        /// Amount of shots released in quick succession
        /// </summary>
        public int bulletsInCharge;
        
        /// <summary>
        /// Whether the weapon is attached to a player
        /// </summary>
        public bool isPlayers;

        /* ------------------ Protected Fields ------------------ */
        
        /// <summary>
        /// Time of the last shot in seconds
        /// </summary>
        protected float TimeOfLastShot;
        
        /// <summary>
        /// Amount of bullets shot in the current charge
        /// </summary>
        protected int BulletsShot = 0;

        /// <summary>
        /// List of IShootable sources attached to this weapon
        /// </summary>
        protected List<IShootable> GunHeads = new();
        
        /// <summary>
        /// Event invoked when the weapon shoots
        /// </summary>
        protected Action OnShootEvent;
        
        /* ------------------ Unity Callbacks ------------------ */
        
        void Start()
        {
            if (isPlayers) Controls.Action1 += TryShooting;
            
            DetectShootables();
            TimeOfLastShot = Time.time - cooldown;
        }

        void Update()
        {
            if (HasAggro) TryShooting();
        }

        
        /* ------------------ Virtual Methods ------------------ */
        
        /// <summary>
        /// Finds all IShootable sources attached to this weapon by searching through its children
        /// </summary>
        protected virtual void DetectShootables()
        {
            var childShootables = transform.GetComponentsInChildren<IShootable>();
            
            foreach (IShootable shootable in childShootables)
            {
                GunHeads.Add(shootable);
                OnShootEvent += shootable.OnShoot;
            }
        }

        /// <summary>
        /// Checks if the weapon can shoot and shoots if it can
        /// </summary>
        protected virtual void TryShooting()
        {
            if (BulletsShot >= bulletsInCharge)
            {
                Recharge();
                return;
            }
            
            if (Time.time - TimeOfLastShot <= cooldown) return;
            
            OnShootEvent?.Invoke();
            TimeOfLastShot = Time.time;
            BulletsShot++;
        }
        
        /* ------------------ Protected Methods ------------------ */
        
        /// <summary>
        /// Wait for the charge time to pass before shooting again
        /// </summary>
        protected void Recharge()
        {
            if (Time.time - TimeOfLastShot <= chargeTime) return;
            BulletsShot = 0;
        }

        /* ------------------ Private Methods ------------------ */
        
        // Unsubscribe from events when the weapon is destroyed
        private void OnDestroy()
        {
            if (isPlayers) Controls.Action1 -= TryShooting;
        }
    }
}