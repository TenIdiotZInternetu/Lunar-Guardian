using System;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// Listens to the Unity's Input Manager and triggers events based on the input
    /// </summary>
    public class Controls : MonoBehaviour
    {
        /*--------------------- Public Fields -------------------*/
        
        /// <summary>
        /// Value of the Input Manager's virtual horizontal axis (A/D)
        /// </summary>
        public static float MoveHorizontal;
        
        /// <summary>
        /// Value of the Input Manager's virtual vertical axis (W/S)
        /// </summary>
        public static float MoveVertical;
        
        /// <summary>
        /// True if the focus button is held down
        /// </summary>
        public static bool IsFocused;
        
        /*------------------------- Events ----------------------*/
        
        /// <summary>
        /// Invoked when the primary action button is pressed (K/Z)
        /// </summary>
        public static event Action Action1;
        
        /// <summary>
        /// Invoked when the primary action button is released (K/Z)
        /// </summary>
        public static event Action Action1Release;
        
        /// <summary>
        /// Invoked when the secondary action button is pressed (L/X)
        /// </summary>
        public static event Action Action2;
        
        /// <summary>
        /// Invoked when the secondary action button is pressed (L/X)
        /// </summary>
        public static event Action Action2Release;
        
        /// <summary>
        /// Invoked when the submit button is pressed (Enter)
        /// </summary>
        public static event Action Submit;
        
        /// <summary>
        /// Invoked when the submit button is released (Enter)
        /// </summary>
        public static event Action SubmitRelease;
        
        /// <summary>
        /// Invoked when the cancel button is pressed (Esc)
        /// </summary>
        public static event Action Cancel;
        
        /// <summary>
        /// Invoked when the cancel button is released (Esc)
        /// </summary>
        public static event Action CancelRelease;
        
        /// <summary>
        /// Invoked when the vertical axis is moved upwards, with its value as a parameter
        /// </summary>
        public static event EventHandler<float> MovesUp;
        
        /// <summary>
        /// Invoked when the vertical axis is moved downwards, with its value as a parameter
        /// </summary>
        public static event EventHandler<float> MovesDown;
        
        /// <summary>
        /// Invoked when the horizontal axis is moved left, with its value as a parameter
        /// </summary>
        public static event EventHandler<float> MovesLeft;
        
        /// <summary>
        /// Invoked when the horizontal axis is moved right, with its value as a parameter
        /// </summary>
        public static event EventHandler<float> MovesRight;
        
        /*----------------------- Unity Callbacks ------------------*/
        
        void Update()
        {
            MoveHorizontal = Input.GetAxis("Horizontal");
            MoveVertical = Input.GetAxis("Vertical");
            
            bool action1 = Input.GetButton("Fire1");
            bool action2 = Input.GetButton("Fire2");
            bool submit = Input.GetButton("Submit");
            bool cancel = Input.GetButton("Cancel");
            
            if (action1) Action1?.Invoke();
            else Action1Release?.Invoke();
            
            if (action2) Action2?.Invoke();
            else Action2Release?.Invoke();
            
            if (submit) Submit?.Invoke();
            else SubmitRelease?.Invoke();
            
            if (cancel) Cancel?.Invoke();
            else CancelRelease?.Invoke();
            
            if (MoveVertical > 0.001f) MovesUp?.Invoke(this, MoveVertical);
            if (MoveVertical < -0.001f) MovesDown?.Invoke(this, MoveVertical);
            if (MoveHorizontal < -0.001f) MovesLeft?.Invoke(this, MoveHorizontal);
            if (MoveHorizontal > 0.001f) MovesRight?.Invoke(this, MoveHorizontal);
            
            IsFocused = Input.GetButton("Fire3");
        }
    }
}