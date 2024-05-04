using UnityEngine;

namespace Spawnables.Weapons
{
    public class GatlingGun : Weapon
    {
        private int _gunHeadIndex = 0;
        private int _gunHeadIncrement = 1;
        
        protected override void TryShooting()
        {   
            if (BulletsShot >= bulletsInCharge)
            {
                Recharge();
                return;
            }
            
            if (Time.time - TimeOfLastShot <= cooldown) return;

            ShiftGunHeadIndex();
            GunHeads[_gunHeadIndex].OnShoot();
            
            TimeOfLastShot = Time.time;
            BulletsShot++;
        }
        
        private void ShiftGunHeadIndex()
        {
            _gunHeadIndex += _gunHeadIncrement;
            
            if (_gunHeadIndex <= 0 || _gunHeadIndex >= GunHeads.Count - 1)
            {
                _gunHeadIncrement *= -1;
            }
        }
    }
}