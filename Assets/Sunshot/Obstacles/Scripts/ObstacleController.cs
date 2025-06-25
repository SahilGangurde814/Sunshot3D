using Sunshot.Gameplay;
using Sunshot.Generic.ObjectPool;
using Sunshot.SolarEnergySystem.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Sunshot.Obstacles
{
    public class ObstacleInfo
    {
        public ObstacleType obstacleType;
        public float spawnCoolDown;
    }

    public class ObstacleController : MonoBehaviour
    {
        public ObstacleInfo obstacleInfo = new();

        // Dependencies
        ObjectPool objectPool;
        GameManager gameManager;

        private void Awake()
        {
            objectPool = ObjectPool.instance;
            gameManager = GameManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag != "Player") return;

            gameManager.GameOver();
        }

        public void Deactivate()
        {
            CancelInvoke();
            objectPool.ReturnObstacle(transform);
        }

        public void SetObstacleInfo(ObstacleType obstacleType, float spawnCoolDown)
        {
            obstacleInfo.obstacleType = obstacleType;
            obstacleInfo.spawnCoolDown = spawnCoolDown;
        }
    }
}
