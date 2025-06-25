using UnityEngine;

namespace Sunshot.CollectibleSystem.Collectibles
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectibles System/Generic Collectible")]
    public class Collectible : ScriptableObject
    {
        public Transform transform;
        public float solarEnergyAmount;
    }
}
