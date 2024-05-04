using System.Reflection;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// A collection of utilities for working with Components
    /// </summary>
    public static class ComponentUtils
    {
        /// <summary>
        /// Overwrites the values of all fields of a component with the values of another component 
        /// </summary>
        /// <param name="original"> The overwritten component </param>
        /// <param name="replacement"> The component with desired values </param>
        /// <typeparam name="T"> Type of the component </typeparam>
        /// <returns> The original component with overwritten values </returns>
        public static T ReplaceComponent<T>(T original, T replacement) where T : Component
        {
            FieldInfo[] fields = typeof(T).GetFields();
            
            foreach (FieldInfo field in fields)
            {
                field.SetValue(original, field.GetValue(replacement));
            }

            return original;
        }
    }
}