using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerScripts
{
    /// <summary>
    /// Defines effects of deploying a bomb by player. The bomb deals gradual damage to all enemies and destroys
    /// projectiles for specified amount of time. The damage is dealt only during frames called damage ticks.
    /// </summary>
    public class BombController :  MonoBehaviour
    {
        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// Damage dealt by the bomb at the beginning of its effect
        /// </summary>
        public float initialDamage;
        
        /// <summary>
        /// Damage multiplier of the bomb in the specified time
        /// </summary>
        public AnimationCurve damageCurve;
        
        /// <summary>
        /// Time between damage ticks
        /// </summary>
        public float damageTickInterval;
        
        /* ---------------------- Events ----------------------- */
        
        /// <summary>
        /// Event invoked every time the bomb deals damage
        /// </summary>
        public static event Action<int> OnBombDamageTick;
        
        /// <summary>
        /// Event invoked when the bomb is deployed by player
        /// </summary>
        public UnityEvent onBombDeployed;
        
        /// <summary>
        /// Event invoked when the bomb stops dealing any more damage
        /// </summary>
        public UnityEvent onBombEffectEnd;


        /* ------------------- Private Fields ------------------ */
        
        // Time when the bomb was deployed in seconds
        private float _timeOfDeployment;
        
        /* ------------------- Public Methods ------------------ */
        
        /// <summary>
        /// Coroutine that deals damage and destroys projectiles on each damage tick
        /// </summary>
        public IEnumerator DeployBomb()
        {
            onBombDeployed?.Invoke();
            
            _timeOfDeployment = Time.time;
            float timeElapsed = 0;

            while (timeElapsed < damageCurve.keys[^1].time)
            {
                timeElapsed = Time.time - _timeOfDeployment;
                float damage = initialDamage * damageCurve.Evaluate(timeElapsed);
                int finalDamage = (int)Math.Ceiling(damage);
                
                OnBombDamageTick?.Invoke(finalDamage);
                yield return new WaitForSeconds(damageTickInterval);
            }
            
            onBombEffectEnd?.Invoke();
        }
    }
}