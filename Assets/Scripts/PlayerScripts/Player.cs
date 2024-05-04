using System;
using System.Collections.Generic;
using GameManager;
using Spawnables;
using Spawnables.Weapons;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Controls player's movement and responses to his actions
    /// </summary>
    public class Player : MonoBehaviour
    {
        /* ------------------- Static Fields ------------------- */
        
        /// <summary>
        /// Singleton instance of the player
        /// </summary>
        public static Player Instance;
        
        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// Distance travelled in one second of held movement key
        /// </summary>
        public float movementSpeed;
        
        /// <summary>
        /// Ratio at which the movement speed is decreased when focused
        /// </summary>
        public float focusSpeedModifier;
        
        /// <summary>
        /// Appropriate BombController activated upon deploying a bomb by the player
        /// </summary>
        public BombController bombBehaviour;
        
        /// <summary>
        /// A set of weapons used by the player at current power level when shooting
        /// </summary>
        public GameObject currentWeaponSet;
        
        /// <summary>
        /// List of weapons used at appropriate power levels
        /// </summary>
        public List<GameObject> weaponSets;
        
        /* ------------------- Private Fields -------------------- */
        
        private Rigidbody2D _rigidbody;          // Rigidbody of the player
        private bool _hasControl = false;        // Whether the player has control over the character
        private bool _hasBombs = true;           // Whether the player has bombs left

        // Whether the player has deployed a bomb of which effects have not ended yet
        private bool _inBombState = false;
        
        /* ------------------ Unity Callbacks ------------------- */
        
        // Sets appropriate responses to player's actions
        void Start()
        {
            Instance = this;
            _rigidbody = GetComponent<Rigidbody2D>();
            
            PlayerStatus.BombsChangedEvent += CheckBombs;
            PlayerStatus.PowerLevelChangedEvent += ChangeWeapon;
            Controls.Action2 += DeployBomb;
        }

        // Determines the velocity of the player
        void FixedUpdate()
        {
            Vector3 movementVector = new Vector3(Controls.MoveHorizontal, Controls.MoveVertical, 0);
            Vector2 velocity = movementVector.normalized * (movementSpeed);
            
            if (Controls.IsFocused) velocity *= focusSpeedModifier;
            
            _rigidbody.velocity = velocity;
        }
        
        /* ------------------ Public Methods ------------------- */
        
        /// <summary>
        /// Changes bomb state
        /// </summary>
        /// <param name="state"> Whether the bomb effects still last </param>
        public void ChangeBombState(bool state)
        {
            _inBombState = state;
        }
        
        /// <summary>
        /// Changes currently used set of weapons
        /// </summary>
        /// <param name="powerLevel"> Player's power level </param>
        public void ChangeWeapon(float powerLevel)
        {
            ChangeWeapon((int) powerLevel);
        }
        
        /// <summary>
        /// Changes currently used set of weapons
        /// </summary>
        /// <param name="powerLevel"> Player's power level </param>
        public void ChangeWeapon(int powerLevel)
        {
            currentWeaponSet.gameObject.SetActive(false);
            currentWeaponSet = weaponSets[powerLevel];
            currentWeaponSet.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Changes whether the player has control over the character
        /// </summary>
        /// <param name="state"> Current GameState of the game </param>
        public void ChangeControl(GameState state)
        {
            _hasControl = state is PlayingState;
        }

        /* ------------------ Private Methods ------------------ */
        
        // Deploys a bomb if the player has control over the character, has bombs left 
        // and there is not a bomb deployed already
        private void DeployBomb()
        {
            if (!_hasControl || !_hasBombs || _inBombState) return;
            StartCoroutine(bombBehaviour.DeployBomb());
            PlayerStatus.Instance.ChangeBombs(-1);
        }
        
        // Checks whether the player has bombs left
        private void CheckBombs(float bombs)
        {
            _hasBombs = bombs > 0;
        }

        // Unsuscribes from events
        private void OnDestroy()
        {
            PlayerStatus.BombsChangedEvent -= CheckBombs;
            PlayerStatus.PowerLevelChangedEvent -= ChangeWeapon;
            Controls.Action2 -= DeployBomb;
        }
    }
}
