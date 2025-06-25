using Sunshot.ShootingSystem.Projectiles;
using System.Collections.Generic;
using UnityEngine;
using Sunshot.Obstacles;
using System;
using Unity.Collections;
using Sunshot.CollectibleSystem.Collectibles;
using Sunshot.CollectibleSystem;

namespace Sunshot.Generic.ObjectPool
{
    [Serializable]
    public class ObstaclePool
    {
        public Obstacle obstacle;
        public int queueSize;
        public Queue<Transform> obstacleQueue = new();
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool instance;

        [Header("Projectile Pool Data")]
        [SerializeField] Projectile selectedProjectile;
        [SerializeField] Transform projectilePoolParent;
        [SerializeField] int projectilePoolSize = 20;
        
        private Queue<Transform> projectileQueue = new();

        [Space]
        
        [Header("Obstacle Pool Data")]
        [SerializeField] Transform obstaclePoolParent;
        [SerializeField] ObstaclePool[] obstaclePool;

        [Space]

        [Header("Collectibles Pool Data")]
        [SerializeField] Collectible selectedCollectible;
        [SerializeField] int collectiblePoolSize;
        [SerializeField] Transform collectiblesParent;

        private Queue<Transform> collectibleQueue = new();


        private void Awake()
        {
            if (instance != null)
                Destroy(instance);
            
            instance = this;
        }

        private void Start()
        {
            // Make pool at start
            PoolSetup();
        }

        void PoolSetup()
        {
            // Projectile pool setup
            for (int i = 0; i < projectilePoolSize; i++)
            {
                Transform projectile = Instantiate(selectedProjectile.transform, projectilePoolParent);
                projectile.gameObject.SetActive(false);
                projectileQueue.Enqueue(projectile);
            }


            // Obstacle pool setup for all types
            foreach(var poolObject in obstaclePool)
            {
                Obstacle currentObstacle = poolObject.obstacle;

                for(int i = 0; i < poolObject.queueSize; i++)
                {
                    Transform obstacle = Instantiate(currentObstacle.obstacleTransform, obstaclePoolParent);

                    obstacle.GetComponent<ObstacleController>().SetObstacleInfo(currentObstacle.obstacleType, currentObstacle.spawnCooldown);
                    obstacle.gameObject.SetActive(false);
                    poolObject.obstacleQueue.Enqueue(obstacle);
                }
            }

            // Collectible Pool seutup
            for(int i = 0; i < collectiblePoolSize; i++)
            {
                Transform collectible = Instantiate(selectedCollectible.transform, collectiblesParent);
                CollectiblesHandler collectiblesHandler = collectible.GetComponent<CollectiblesHandler>();
                collectiblesHandler.SetCollectiblesData(selectedCollectible.solarEnergyAmount);

                collectible.gameObject.SetActive(false);
                collectibleQueue.Enqueue(collectible);
            }
        }
        public Transform GetProjectile() => GetObjectFromPool(projectileQueue, selectedProjectile.transform, projectilePoolParent);
        public Transform GetObstacleFromPool(ObstacleType obstacleType)
        {
            Transform obstacle = null;

            foreach(var poolObject in obstaclePool)
            {
                if(poolObject.obstacle.obstacleType == obstacleType)
                {
                    obstacle = GetObjectFromPool(poolObject.obstacleQueue, poolObject.obstacle.obstacleTransform, obstaclePoolParent);

                    return obstacle;
                }
            }

            return obstacle;
        }

        public Transform GetCollectible() => GetObjectFromPool(collectibleQueue, selectedCollectible.transform, collectiblesParent);

        private Transform GetObjectFromPool(Queue<Transform> queue, Transform objectTransform, Transform parent)
        {
            Transform currentObject;

            if (queue.Count == 0)
            {
                currentObject = Instantiate(objectTransform, parent);

                return currentObject;
            }

            currentObject = queue.Dequeue();

            return currentObject;
        }


        public void ReturnBullet(Transform Object)
        {
            ReturnObject(Object, projectileQueue);
        }

        public void ReturnObstacle(Transform Object)
        {
            ObstacleType obstacleType = Object.GetComponent<ObstacleController>().obstacleInfo.obstacleType;

            foreach(var poolObject in obstaclePool)
            {
                if(poolObject.obstacle.obstacleType == obstacleType)
                {
                    ReturnObject(Object, poolObject.obstacleQueue);
                    return;
                }
            }
        }

        public void ReturnCollectible(Transform Object)
        {
            ReturnObject(Object, collectibleQueue);
        }

        private void ReturnObject(Transform Object, Queue<Transform> queue)
        {
            Object.gameObject.SetActive(false);
            queue.Enqueue(Object);
        }
    }
}
