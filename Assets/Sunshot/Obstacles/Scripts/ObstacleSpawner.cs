using Sunshot.Generic.ObjectPool;
using UnityEngine;

namespace Sunshot.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] Transform obstacleSpawnPoint;
        [SerializeField] float coolDownTime = 1f;

        [Header("Obstacle Spawn area")]
        [SerializeField] float minX = -4f;
        [SerializeField] float maxX = 4f;
        [SerializeField] float minY = 0.5f;
        [SerializeField] float maxY = 4.5f;


        //Dependencies
        ObjectPool objectPool;

        float elapsedTime = 0f;
        ObstacleType nextObstacleTypeToSpawn = ObstacleType.Destructible;

        private void Start()
        {
            objectPool = ObjectPool.instance;
        }

        private void Update()
        {
            SpawnObstacle();
        }

        void SpawnObstacle()
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime < coolDownTime) return;

            Transform currentObstacle = objectPool.GetObstacleFromPool(nextObstacleTypeToSpawn);

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector3 randomOffset = new Vector3(randomX, randomY, obstacleSpawnPoint.position.z);

            currentObstacle.position = randomOffset;
            currentObstacle.gameObject.SetActive(true);

            elapsedTime = 0f;
            
            nextObstacleTypeToSpawn = GetRandomObstacleType();
        }

        ObstacleType GetRandomObstacleType()
        {
            ObstacleType obstacleType;

            ObstacleType[] ObstacleTypes = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));
            int randomIndex = Random.Range(1, ObstacleTypes.Length);
            obstacleType = ObstacleTypes[randomIndex];

            return obstacleType;
        }
    }
}
