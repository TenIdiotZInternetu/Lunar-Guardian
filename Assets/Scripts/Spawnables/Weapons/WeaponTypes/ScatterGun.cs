using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawnables.Weapons
{
    public class ScatterGun : Weapon
    {
        public List<Projectile> projectileRoster;
        public AnimationCurve rotationAtSpawnDistribution;
        
        protected override void TryShooting()
        {
            if (BulletsShot >= bulletsInCharge)
            {
                Recharge();
                return;
            }
            
            StartCoroutine(Scatter());
            TimeOfLastShot = Time.time;
        }

        private IEnumerator Scatter()
        {
            while (BulletsShot < bulletsInCharge)
            {
                RotateSpawners();
                ChooseProjectile();
                OnShootEvent?.Invoke();
                BulletsShot++;
                yield return new WaitForSeconds(cooldown);
            }
        }

        private void RotateSpawners()
        {
            foreach (IShootable shootable in GunHeads)
            {
                var gunHead = shootable as MonoBehaviour;

                if (gunHead == null)
                {
                    throw new ArgumentException($"{shootable} is not an instance of MonoBehaviour");
                }
                
                Transform gunHeadTransform = gunHead.transform;
                
                float rotationAtSpawn = rotationAtSpawnDistribution.Evaluate(Random.value);
                Quaternion rotation = Quaternion.Euler(0, 0, rotationAtSpawn);
                
                gunHeadTransform.localRotation = rotation;
            }
        }

        private void ChooseProjectile()
        {
            int index = Random.Range(0, projectileRoster.Count);
            Projectile projectile = projectileRoster[index];
            
            foreach (IShootable gunHead in GunHeads)
            {
                BulletSpawner bulletSpawner = gunHead as BulletSpawner;
                if (bulletSpawner == null) continue;
                
                bulletSpawner.projectile = projectile.gameObject;
            }
        }
    }
}