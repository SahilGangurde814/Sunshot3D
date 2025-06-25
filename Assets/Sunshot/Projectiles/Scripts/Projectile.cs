using UnityEngine;

namespace Sunshot.ShootingSystem.Projectiles
{
    public enum ProjectileType
    {
        None, Light, Heavy
    }

    [CreateAssetMenu(fileName = "Projectile", menuName = "Shooting System/Generic Projectile")]
    public class Projectile : ScriptableObject
    {
        public ProjectileType projectileType;

        public string projectileName;
        public Transform transform;
        public float coolDownTime;
        public float damage;
    }
}
