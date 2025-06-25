using Sunshot.Generic.ObjectPool;
using UnityEngine;

namespace Sunshot.CollectibleSystem
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] float coolDownTime = 10f;
        [SerializeField] Transform collectibleSpawnPoint;
        private float elapsedTime;

        // Dependencies
        private ObjectPool objectPool;

        private void Start()
        {
            objectPool = ObjectPool.instance;
        }

        private void Update()
        {
            SpawnCollectibles();
        }

        void SpawnCollectibles()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < coolDownTime) return;

            Transform currentCollectible = objectPool.GetCollectible();

            float randomX = Random.Range(-4, 4);
            float randomY = Random.Range(1, 4.5f);

            Vector3 randomOffset = new Vector3(randomX, randomY, collectibleSpawnPoint.position.z);

            currentCollectible.position = randomOffset;
            currentCollectible.gameObject.SetActive(true);

            elapsedTime = 0f;
        }
    }
}
