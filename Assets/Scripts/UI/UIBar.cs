using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// A bar shown during gameplay that displays a resource
    /// </summary>
    public class UIBar : MonoBehaviour
    {
        /* ------------------ Public Fields ------------------ */
        
        /// <summary>
        /// Resourse to display
        /// </summary>
        public PlayerStatus.ResourceType resourceType;
            
        /* ------------------ Private Fields ----------------- */
        
        private Slider _slider;             // Slider component representing the bar

        /* ------------------ Unity Callbacks ---------------- */
        
        // Subscribe to the PlayerStatus event of the specified resource type
        void Start()
        {
            PlayerStatus.Subscribe(resourceType, ChangeValue);
            _slider = GetComponent<Slider>();
        }
        
        /* ------------------ Public Methods ----------------- */
        
        /// <summary>
        /// Changes highest displayed value, effectively scaling the bar
        /// </summary>
        /// <param name="value"> The new highest value of the bar </param>
        public void ChangeMaxValue(int value)
        {
            _slider.maxValue = value;
            
        }

        /* ------------------ Private Methods ---------------- */
        
        /// <summary>
        /// Changes the value of the bar
        /// </summary>
        /// <param name="value"> The value to be displayed </param>
        private void ChangeValue(float value)
        {
            _slider.value = value;
        }
    }
}