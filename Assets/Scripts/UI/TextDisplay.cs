using System;
using System.Text;
using MyBox;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

namespace UI
{
    /// <summary>
    /// Tracks a PlayerStatus resource and displays it as formatted text
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class TextDisplay : MonoBehaviour
    {
        /* ------------------ Constants ------------------ */
        
        /// <summary>
        /// Value to be used for previewing in the editor
        /// </summary>
        private const float PREVIEW_VALUE = 1.23f;
        
        /* ---------------- Public Fields ---------------- */
        
        /// <summary>
        /// Resource to track and display
        /// </summary>
        public PlayerStatus.ResourceType trackedResource;
        
        /// <summary>
        /// Format of the final string
        /// </summary>
        public string format = "{0}";
        
        /// <summary>
        /// Initial value of the resource
        /// </summary>
        public float defaultValue = 0;
        
        /// <summary>
        /// Whether to hide the text when the value is the default
        /// </summary>
        public bool hideWhenDefault = false;
        
        /// <summary>
        /// Whether to add separators between groups of digits
        /// </summary>
        public bool addSeparators = false;
        
        /// <summary>
        /// Number of digits between separators
        /// </summary>
        [ConditionalField(nameof(addSeparators))]
        public int separatorInterval = 3;
        
        /// <summary>
        /// Amount of zeros to pad the value wit
        /// </summary>
        public int zeroPadding = 0;

        /* ------------------ Private Fields ----------------- */
        
        private TMP_Text _tmpComponent;                 // Text component to be updated
        private StringBuilder _stringBuilder = new();   // StringBuilder to be used for formatting

        /* ------------------ Unity Callbacks ---------------- */
        
        // Subscribe to the PlayerStatus event of the specified resource type
        void Start()
        {
            _tmpComponent = GetComponent<TMP_Text>();
            PlayerStatus.Subscribe(trackedResource, UpdateText);
            UpdateText(defaultValue);
        }
        
        void OnValidate()
        {
            if (_tmpComponent == null)
            {
                _tmpComponent = GetComponent<TMP_Text>();
            }
            
            UpdateText(PREVIEW_VALUE);
        }
        
        /* ------------------ Private Methods ---------------- */
        
        // Updates the value and adds padding and seperators
        private void UpdateText(float value)
        {
            if (hideWhenDefault && value == defaultValue)
            {
                _tmpComponent.text = "";
                return;
            }
            
            _stringBuilder.Clear();
            string targetString = value.ToString();
            
            targetString = targetString.PadLeft(zeroPadding, '0');
            _stringBuilder.Append(targetString);

            if (addSeparators) AddSeparators();
            
            string newText = String.Format(format, _stringBuilder);
            _tmpComponent.text = newText;
        }
        
        // Adds separators to the string
        private void AddSeparators()
        {
            if (separatorInterval <= 0) return;
            // if (separatorInterval == null) return;
            
            int length = _stringBuilder.Length;
            int offset = length % separatorInterval;
            
            if (offset == 0) offset = separatorInterval;

            for (int i = offset; i < _stringBuilder.Length; i += separatorInterval + 1)
            {
                _stringBuilder.Insert(i, '.');
            }
        }
    }
}