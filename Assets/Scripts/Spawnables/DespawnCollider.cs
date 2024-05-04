using UnityEngine;

namespace Spawnables
{
    /// <summary>
    /// Border collider that despawns objects that leave its area
    /// </summary>
    public class DespawnCollider : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.activeSelf) return;
            ObjectPoolManager.Despawn(other.gameObject);
        }
    }
}
