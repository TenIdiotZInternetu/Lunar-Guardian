using System;
using GameManager;
using UnityEngine;
using UnityEngine.Events;

namespace Tools
{
    /// <summary>
    /// Unity Event with GameObject parameter
    /// </summary>
    [Serializable] public class GameObjectEvent : UnityEvent<GameObject> {}
    
    /// <summary>
    /// Unity Event with GameState parameter
    /// </summary>
    [Serializable] public class GameStateEvent : UnityEvent<GameState> {}
}