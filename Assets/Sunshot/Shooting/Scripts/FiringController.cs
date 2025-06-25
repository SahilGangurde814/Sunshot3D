using UnityEngine;
using Sunshot.ShootingSystem.Projectiles;
using System.Collections.Generic;
using Sunshot.Generic.ObjectPool;

namespace Sunshot.ShootingSystem
{
    public class FiringController : MonoBehaviour
    {
        [SerializeField] Projectile selectedProjectile;
        [SerializeField] Transform spawnPoint;

        float elapsedTime = 0f;

        // Dependencies
        private ObjectPool objectPool;

        private void Start()
        {
            objectPool = ObjectPool.instance;
        }

        private void Update()
        {
            AutoFire();
        }

        void AutoFire()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < selectedProjectile.coolDownTime) return;

            Transform currentProjectile = objectPool.GetProjectile();

            currentProjectile.position = spawnPoint.position;
            currentProjectile.gameObject.SetActive(true);

            elapsedTime = 0f;
        }
    }
}
