using UnityEngine;

namespace Sunshot.ShootingSystem.Projectiles
{
    [CreateAssetMenu(fileName = "HeavyProjectile", menuName = "Shooting System/Heavy Projectile")]
    public class HeavyProjectile : Projectile
    {
        public HeavyProjectile()
        {
            projectileType = ProjectileType.Heavy;
            coolDownTime = 0.5f;
            damage = 2f;
        }
    }
}
