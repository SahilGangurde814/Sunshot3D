using Sunshot.Generic.ObjectPool;
using Sunshot.PlayerSystem;
using Sunshot.SolarEnergySystem.UI;
using UnityEngine;

namespace Sunshot.CollectibleSystem
{
    public class CollectibleData
    {
        public float solarEnergyAmout;
    }

    public class CollectiblesHandler : MonoBehaviour
    {
        private CollectibleData collectibleData = new();

        // Dependencies
        private HUDmanager hudManger;
        private ObjectPool ObjectPool;

        private void Start()
        {
            hudManger = HUDmanager.instance;
            ObjectPool = ObjectPool.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") != true) return;
            
            hudManger.SetSolarEnergy(collectibleData.solarEnergyAmout);

            Deactivate();
        }

        public CollectibleData GetCollectiblesData()
        {
            return collectibleData;
        }

        public void SetCollectiblesData(float solarEnergyAmount)
        {
            collectibleData.solarEnergyAmout = solarEnergyAmount;
        }

        public void Deactivate()
        {
            CancelInvoke();
            ObjectPool.ReturnCollectible(transform);
        }
    }
}
