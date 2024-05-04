using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    [CreateAssetMenu(fileName = "Credits", menuName = "ScriptableObjects/Credits", order = 1)]
    public class CreditsSo : ScriptableObject
    {
        [Serializable]
        public class Credit
        {
            public string name;
            public string role;
        }
        
        public List<Credit> credits = new();
    }
}