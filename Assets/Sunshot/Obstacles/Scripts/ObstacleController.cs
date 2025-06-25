using Sunshot.Gameplay;
using Sunshot.Generic.ObjectPool;
using Sunshot.SolarEnergySystem.UI;
using System.Collections;
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
        public GameObject destroyParticle;
    }

    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] ParticleSystem destroyParticle;

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

        public void Deactivate(bool showDestroyParticle)
        {
            CancelInvoke();
            
            if(showDestroyParticle)
                ActiveDestroyParticle();
            else
                objectPool.ReturnObstacle(transform);
        }

        void ActiveDestroyParticle()
        {
            if(obstacleInfo.obstacleType != ObstacleType.Destructible) return;

            destroyParticle.gameObject.SetActive(true);
            destroyParticle.Play();
            StartCoroutine(WaitForSeconds(0.3f));
        }

        IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            objectPool.ReturnObstacle(transform);
        }

        public void SetObstacleInfo(ObstacleType obstacleType, float spawnCoolDown)
        {
            obstacleInfo.obstacleType = obstacleType;
            obstacleInfo.spawnCoolDown = spawnCoolDown;
        }
    }
}
