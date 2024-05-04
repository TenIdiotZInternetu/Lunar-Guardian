using System;
using Spawnables;
using Spawnables.EnemyScripts;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    /// <summary>
    /// Takes responsibility and responses to being hit by enemy objects
    /// </summary>
    public class PlayerHitbox : MonoBehaviour
    {
        /*----------------- Public Fields -----------------*/
        
        /// <summary>
        /// Time window in which the player cannot take damage after taking a hit
        /// </summary>
        public float invincibilityTime;
        
        /// <summary>
        /// Invoked when the player takes a hit
        /// </summary>
        public GameObjectEvent onTakesHitEvent;
        
        /*----------------- Private Fields -----------------*/
        
        private bool _inBombState;          // Whether the player has deployed a bomb
        private float _timeOfLastHit;       // Exact time in seconds

        /*----------------- Unity Callbacks -----------------*/
        
        // Checks whether the player has been hit by an enemy object, and despawns it or damages it
        void OnTriggerEnter2D(Collider2D other)
        {
            if (IsInvincible()) return;
            
            GameObject collidedObject = other.gameObject;
            bool isProjectile = collidedObject.CompareTag("EnemyProjectile");
            bool isEnemy = collidedObject.CompareTag("Enemy");
            
            if (!(isProjectile || isEnemy)) return; 
            
            AttemptHit(collidedObject);

            if (isProjectile)
            {
                ObjectPoolManager.Despawn(collidedObject);
            }
            
            if (isEnemy)
            {
                collidedObject.GetComponent<EnemyController>().TakeDamage(50, this.gameObject);
            }
        }
        
        /*----------------- Public Methods -----------------*/
        
        /// <summary>
        /// Deals damage to player if they're not invincible
        /// </summary>
        /// <param name="damageSource"> Object causing the damage </param>
        public void AttemptHit(GameObject damageSource)
        {
            if (IsInvincible()) return;
            TakeDamage(damageSource);
        }
        
        /// <summary>
        /// Changes bomb state
        /// </summary>
        /// <param name="state"> Whether the bomb effects still last </param>
        public void ChangeBombState(bool state)
        {
            _inBombState = state;
        }

        
        /*----------------- Private Methods -----------------*/
        
        // Checks if the player can be damaged
        private bool IsInvincible()
        {
            return _inBombState || Time.time - _timeOfLastHit <= invincibilityTime;
        }
        

        // Decreases player's health and resets their score multiplier
        private void TakeDamage(GameObject damageSource)
        {
            PlayerStatus.Instance.ResetScoreMultiplier();
            PlayerStatus.Instance.ChangeHealth(-1);
            
            onTakesHitEvent?.Invoke(damageSource);
            _timeOfLastHit = Time.time;
        }
        
    }
}