namespace Spawnables.Weapons
{
    /// <summary>
    /// Contract for entities activated by a weapon, able to hurt either enemies or the player
    /// </summary>
    public interface IShootable
    {
        /// <summary>
        /// Called when the weapon is activated
        /// </summary>
        public void OnShoot();
    }
}