using Sunshot.Generic.ObjectPool;
using Sunshot.Obstacles;
using UnityEngine;

namespace Sunshot.ShootingSystem.Projectiles
{
    public class ProjectileHandler : MonoBehaviour
    {
        [SerializeField] float speed = 20f;
        [SerializeField] float lifeTime = 2f;

        // Dependencies
        ObjectPool objectPool;

        private void Awake()
        {
            objectPool = ObjectPool.instance;
        }

        private void OnEnable()
        {
            Invoke(nameof(Deactivate), lifeTime);
        }

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            ObstacleController obstacleController = collision.transform.GetComponent<ObstacleController>();
            if(obstacleController == null) return;
            
            // Disable projectile and return it to object pool
            Deactivate();

            if(obstacleController.obstacleInfo.obstacleType == ObstacleType.Industructible) return;
            
            // Disable obsatcle and return it to object pool
            obstacleController.Deactivate(true);
        }

        void Deactivate()
        {
            CancelInvoke();
            objectPool.ReturnBullet(transform);
        }
    }
}
