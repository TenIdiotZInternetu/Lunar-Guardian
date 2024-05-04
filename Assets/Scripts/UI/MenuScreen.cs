using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    /// <summary>
    /// A menu screen's functionality
    /// </summary>
    public class MenuScreen : MonoBehaviour
    {
        /*----------------- Public Fields -----------------*/
        
        /// <summary>
        /// Amount of time to wait before deactivating the menu, to let VFX finish playing
        /// </summary>
        public float onCloseDelay = 0.3f;
        
        /*----------------- Events -----------------*/
        
        /// <summary>
        /// Event invoked when the menu is closed
        /// </summary>
        public UnityEvent menuLeftEvent;
        
        /// <summary>
        /// Event invoked when the menu is opened
        /// </summary>
        public UnityEvent menuOpenedEvent;
        
        /*----------------- Public Methods -----------------*/
        
        /// <summary>
        /// Sets the menu to be active and informs listeners
        /// </summary>
        public void OpenMenu()
        {
            gameObject.SetActive(true);
            menuOpenedEvent?.Invoke();
        }

        /// <summary>
        /// Informs listeners and wait for them to finish before deactivating the menu
        /// </summary>
        public void CloseMenu()
        {
            StartCoroutine(CloseCoroutine());
        }
        
        /*----------------- Private Methods -----------------*/
        
        // Split from CloseMenu() to allow for a delay
        private IEnumerator CloseCoroutine()
        {
            menuLeftEvent?.Invoke();
            yield return new WaitForSeconds(onCloseDelay);
            gameObject.SetActive(false);
        }
    }
}