using System;
using PlayerScripts;
using Spawnables.Pickups;
using Spawnables.Weapons;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Spawnables.EnemyScripts
{
    /// <summary>
    /// Describes logic behind all of the enemies
    /// </summary>
    [RequireComponent(typeof(Enemy))]
    public class EnemyController : MonoBehaviour
    {
        /* ------------------- Constants ------------------- */
        
        private const string PLAYER_PROJECTILE_TAG = "PlayerProjectile";    // Tag for recognizing player projectiles
        private const string BORDER_TAG = "PlayfieldBorder";                // Tag for recognizing playfield borders
        private const float TRIGGER_DEBOUNCE_TIME = 0.01f;                  // Time in seconds between OnTrigger calls
                                                                            // to prevent double aggro switches
                                                                            
        /* ----------------- Public Fields ----------------- */
                                                     
        /// <summary>
        /// Whether the Enemy shoots or not
        /// </summary>
        public bool hasAggro = false;
        
        /// <summary>
        /// Weapon used when the enemy shoots
        /// </summary>
        public Weapon weapon;
        
        /// <summary>
        /// Event invoked when the enemy shoots
        /// </summary>
        public event Action ShootsEvent;
    
        /* ----------------- Events ----------------- */
        
        /// <summary>
        /// Event invoked with GameObject parameter when the enemy takes damage
        /// </summary>
        public GameObjectEvent onTakesHitEvent;
        
        /// <summary>
        /// Event invoked when the enemy dies
        /// </summary>
        public UnityEvent onDeathEvent;
        
        /* ----------------- Private Fields ----------------- */
        
        private Enemy _enemyVariables;          // Characteristics of the enemy from the Enemy class
        private int _currentHealth;             // Current health of the enemy
        private float _timeOfLastTrigger;       // Exact time of last OnTrigger call in seconds
        
        /* ----------------- Unity Callbacks ----------------- */

        void OnEnable()
        {
            _enemyVariables = GetComponent<Enemy>();
            _currentHealth = _enemyVariables.maxHealth;
            BombController.OnBombDamageTick += OnBombDamageTick;

            if (weapon != null)
            {
                weapon.HasAggro = hasAggro;
            }
        }

        void Update()
        {
            if (_enemyVariables.tracksPlayer) RotateTowardsPlayer();
        }
    
        // Checks whether the enemy has been hit by a player projectile, and despawns it or damages it
        void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time - _timeOfLastTrigger < TRIGGER_DEBOUNCE_TIME) return;
        
            GameObject collidedObject = other.gameObject;
            if (collidedObject.CompareTag(PLAYER_PROJECTILE_TAG))
            {
                OnProjectileCollision(collidedObject);
            }
        
            _timeOfLastTrigger = Time.time;

        }

        // Checks whether the enemy has crossed the playfield border, and switches aggro if so
        void OnTriggerExit2D(Collider2D other)
        {
            if (Time.time - _timeOfLastTrigger < TRIGGER_DEBOUNCE_TIME) return;
        
            GameObject collidedObject = other.gameObject;
            if (collidedObject.CompareTag(BORDER_TAG)) SwitchAggro();
        
            _timeOfLastTrigger = Time.time;
        }
        
        /* ----------------- Public Methods ----------------- */

        /// <summary>
        /// Decreases the enemy's health by the given amount, and kills it if it reaches 0
        /// </summary>
        /// <param name="damage"> Amount of damage dealt </param>
        /// <param name="damageSource"> Object causing the damage </param>
        public void TakeDamage(int damage, GameObject damageSource)
        {
            if (!isActiveAndEnabled) return;

            onTakesHitEvent?.Invoke(damageSource);
            _currentHealth -= damage;
            if (_currentHealth <= 0) Die();
        }

        
        /* ----------------- Private Methods ----------------- */
        private void OnProjectileCollision(GameObject projectile)
        {
            var projectileScript = projectile.GetComponent<Projectile>();
            TakeDamage(projectileScript.damage, projectile);
            ObjectPoolManager.Despawn(projectile);
        }

        // Changes the enemy's aggro state and notifies the weapon
        private void SwitchAggro()
        {
            if (weapon == null) return;
            hasAggro = true;
            weapon.HasAggro = hasAggro;
        }

        // Rotates the enemy towards the player's current position
        private void RotateTowardsPlayer()
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Quaternion currentRotation  = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, playerPosition - transform.position);
        
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, 360 * Time.deltaTime);
        }

        // Despawns the enemy and awards player with score and loot
        private void Die()
        {
            DropLoot();
            hasAggro = false;
            PlayerStatus.Instance.ChangeScore(_enemyVariables.scoreReward);
            ObjectPoolManager.Despawn(gameObject);
        
            onDeathEvent?.Invoke();
        }

        // Has a chance to drop each of the loot items on a random position around the enemy. 
        private void DropLoot()
        {
            foreach (var drop in _enemyVariables.drops)
            {
                for (int i = 0; i < drop.maxDrops; i++)
                {
                    Vector3 roll = UnityEngine.Random.insideUnitCircle;
                    if (Mathf.Abs(roll.x) >= drop.dropChance) continue;

                    // Normalize x into range from -1 to 1
                    roll.x /= drop.dropChance;
                
                    ObjectPoolManager.Spawn<Pickup>(drop.item, transform.position + roll, Quaternion.identity);
                }
            }
        }
    
        // Deals damage to the enemy on BombController's damage ticks
        private void OnBombDamageTick(int damage)
        {
            TakeDamage(damage, Player.Instance.gameObject);
        }

        // Unsubscribes from BombController's damage ticks
        private void OnDestroy()
        {
            BombController.OnBombDamageTick -= OnBombDamageTick;
        }
    }
}
