using System;
using System.Reflection;
using MyBox;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Records state of player's resources and provides methods to change them
    /// </summary>
    public class PlayerStatus : MonoBehaviour
    {
        /*--------------------- Types ---------------------*/
        
        /// <summary>
        /// Type of resource maintained by the PlayerStatus
        /// </summary>
        public enum ResourceType
        {
            Health,
            Bombs,
            Power,
            Score,
            ScoreMultiplier
        }
        
        /*------------------- Constants -------------------*/
        
        // Substrings to attach when Subscribing to resource agnostic Subscribe and ChangeResource methods
        private const string CHANGE_METHOD_PREFIX = "Change";
        private const string EVENT_NAME_SUFFIX = "ChangedEvent";
        
        // Amount of power needed to increase power level
        // Increasing power level unlocks more powerful weapons
        private readonly int[] POWER_LEVELS = 
        {
            25, 50, 75, 100, 150, 200
        };
        
        // Score multiplier at each level
        private readonly float[] MULTIPLIER_LEVELS = 
        {
            1, 1.15f, 1.3f, 1.45f, 1.6f, 1.75f, 2, 2.2f, 2.4f, 2.6f, 2.8f, 3, 3.5f, 4, 4.5f, 5
        };
        
        /*----------------- Static Fields -----------------*/
        
        /// <summary>
        /// Singleton instance of the PlayerStatus
        /// </summary>
        public static PlayerStatus Instance { get; private set; }
        
        /*-------------- Inspector variables ---------------*/

        
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private int startingHealth = 3;
        [SerializeField] private int maxBombsHeld = 5;
        [SerializeField] private int startingBombsHeld = 2;
        [SerializeField] private int maxPower = 200;
        [SerializeField] private int startingPower = 0;
        
        /// <summary>
        /// Score gained for catching Multiplier pickups while at max multiplier
        /// </summary>
        [Separator]
        [SerializeField] private int maxMultiplierScoreBonus = 100;
        
        /// <summary>
        /// Amount of score required to gain extra health
        /// </summary>
        [SerializeField] private int healthUpRequirementIncrement = 1000000;
        
        /// <summary>
        /// Score gained for gaining health while already at max health
        /// </summary>
        [SerializeField] private int healthOverflowBonus = 20000;

        /*---------------------- Events ---------------------*/
        
        /// <summary>
        /// Method signature for events that are invoked upon a resource change
        /// </summary>
        public delegate void ChangedValueListener(float value);
        
        public static event ChangedValueListener HealthChangedEvent;
        public static event ChangedValueListener BombsChangedEvent;
        public static event ChangedValueListener PowerChangedEvent;
        public static event ChangedValueListener PowerLevelChangedEvent;
        public static event ChangedValueListener ScoreChangedEvent;
        public static event ChangedValueListener ScoreMultiplierChangedEvent;
        
        /// <summary>
        /// Invoked when the player's health reaches less than 0
        /// </summary>
        public static event Action GameOverEvent;
        
        /*------------------ Private Fields -----------------*/

        private int _health;                    // Current health of the player
        private static int _bombsHeld;          // Current amount of bombs held by the player
        private int _power;                     // Current power of the player
        
        private float _scoreMultiplier;         // Current score multiplier of the player
        private int _scoreMultiplierLevel;      // Amount of Multiplier pickups caught in a row
        private int _healthUpRequirement;       // Next extra health gained at this score

        public int Score { get; private set; }

        /* ---------------- Unity Callbacks ---------------- */
        
        private void Start()
        {
            Instance = this;
            Reset();
        }
        
        /* ------------------ Static Methods ----------------- */
        
        /// <summary>
        /// Subscribes a listener to an appropriate resource change event
        /// </summary>
        /// <param name="resourceType"> Type of resource the listener subscribes to </param>
        /// <param name="listener"> Method that responses to triggering of the appropriate event </param>
        public static void Subscribe(ResourceType resourceType, ChangedValueListener listener)
        {
            Type playerStatusType = typeof(PlayerStatus);
            string eventName = resourceType + EVENT_NAME_SUFFIX;
            EventInfo statusEvent = playerStatusType.GetEvent(eventName);
            statusEvent.AddEventHandler(null, listener);
        }
        
        /// <summary>
        /// Change a resource by a given amount
        /// </summary>
        /// <param name="resourceType"> Resource to change </param>
        /// <param name="amount"></param>
        public static void ChangeResource(ResourceType resourceType, int amount)
        {
            Type playerStatusType = typeof(PlayerStatus);
            string methodName = CHANGE_METHOD_PREFIX + resourceType;
            MethodInfo statusMethod = playerStatusType.GetMethod(methodName);
            statusMethod.Invoke(Instance, new object[] {amount});
        }
        
        /* ------------------ Public Methods ------------------- */

        /// <summary>
        /// Sets all resources to their starting values
        /// </summary>
        public void Reset()
        {
            _health = startingHealth;
            HealthChangedEvent?.Invoke(_health);
            
            _bombsHeld = startingBombsHeld;
            BombsChangedEvent?.Invoke(_bombsHeld);
            
            _power = startingPower;
            PowerChangedEvent?.Invoke(_power);
            
            Score = 0;
            ScoreChangedEvent?.Invoke(Score);
            
            _scoreMultiplier = 1;
            ScoreMultiplierChangedEvent?.Invoke(_scoreMultiplier);
            
            _scoreMultiplierLevel = 0;
            _healthUpRequirement = healthUpRequirementIncrement;
        }

        /* ------------------ Public Methods ------------------ */
        
        /// <summary>
        /// Increases health. If health is already at max, increases score instead.
        /// When health reaches 0, invokes GameOverEvent
        /// </summary>
        /// <param name="amount"> Amount of health to be changed </param>
        public void ChangeHealth(int amount)
        {
            _health += amount;

            if (_health > maxHealth)
            {
                ChangeScore(healthOverflowBonus);
                _health = maxHealth;
            }
            
            if (_health < 0) GameOverEvent?.Invoke();
            
            HealthChangedEvent?.Invoke(_health);
        }
        
        /// <summary>
        /// Increases bombs held by a given amount
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeBombs(int amount)
        {
            _bombsHeld += amount;
            Math.Clamp(_bombsHeld, 0, maxBombsHeld);
            BombsChangedEvent?.Invoke(_bombsHeld);
        }
        
        /// <summary>
        /// Increases power and changes power level if POWER_LEVELS requirement is met
        /// </summary>
        /// <param name="amount"> Amount of health to be changed </param>
        public void ChangePower(int amount)
        {
            _power += amount;
            Math.Clamp(_power, 0, maxPower);
            PowerChangedEvent?.Invoke(_power);
            
            for (int i = 0; i < POWER_LEVELS.Length; i++)
            {
                int level = POWER_LEVELS.Length - i;
                
                if (_power >= POWER_LEVELS[level - 1])
                {
                    PowerLevelChangedEvent?.Invoke(level);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Increases score, rewards extra health if requirement are met
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeScore(int amount)
        {
            Score += (int)Math.Ceiling(amount * _scoreMultiplier);

            if (Score >= _healthUpRequirement)
            {
                _healthUpRequirement += healthUpRequirementIncrement;
                ChangeHealth(1);
            }
                
            ScoreChangedEvent?.Invoke(Score);
        }
        
        /// <summary>
        /// Changes score multiplier, awards bonus score if max multiplier is reached
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeScoreMultiplier(int amount)
        {
            if (_scoreMultiplierLevel >= MULTIPLIER_LEVELS.Length - 1)
            {
                ChangeScore(maxMultiplierScoreBonus);
                return;
            }
            
            _scoreMultiplierLevel += amount;
            _scoreMultiplier = MULTIPLIER_LEVELS[_scoreMultiplierLevel];
            ScoreMultiplierChangedEvent?.Invoke(_scoreMultiplier);
        }
        
        /// <summary>
        /// Sets Score Multiplier back to 1
        /// </summary>
        public void ResetScoreMultiplier()
        {
            _scoreMultiplierLevel = 0;
            ChangeScoreMultiplier(0);
        }

        /* ------------------ Private Methods ------------------ */
        
        // Unsubscribes all event listeners when this object is destroyed
        private void OnDestroy()
        {
            HealthChangedEvent = null;
            BombsChangedEvent = null;
            PowerChangedEvent = null;
            PowerLevelChangedEvent = null;
            ScoreChangedEvent = null;
            ScoreMultiplierChangedEvent = null;
            GameOverEvent = null;
        }
    }
}