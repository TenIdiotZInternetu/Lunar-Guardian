using PlayerScripts;

namespace Spawnables.Weapons
{
    /// <summary>
    /// An entity whose purpose is to deal damage to enemies or the player
    /// </summary>
    public class Projectile : Entity
    {
        /* ------------------ Public Variables ------------------ */
        
        /// <summary>
        /// Damage dealt to enemies when owned by the player
        /// </summary>
        public int damage;

        /* ------------------- Unity Callbacks ------------------ */
        
        void Start()
        {
            BombController.OnBombDamageTick += OnBombDamageTick;
        }
        
        /* ------------------ Private Methods ------------------ */
        
        // Despawns the projectile
        private void Disperse()
        {
            ObjectPoolManager.Despawn(gameObject);
        }
        
        // Called on BombController's damage tick
        private void OnBombDamageTick(int damage)
        {
            Disperse();
        }
    }
}