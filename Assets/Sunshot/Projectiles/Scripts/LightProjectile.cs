using UnityEngine;

namespace Sunshot.ShootingSystem.Projectiles
{
    [CreateAssetMenu(fileName = "LightProjectile", menuName = "Shooting System/Light Projectile")]
    public class LightProjectile : Projectile
    {
        public LightProjectile()
        {
            projectileType = ProjectileType.Light;
            coolDownTime = 0.3f;
            damage = 1f;
        }
    }
}
