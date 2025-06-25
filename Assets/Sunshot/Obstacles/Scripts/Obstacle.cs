using UnityEngine;

namespace Sunshot.Obstacles
{
    public enum ObstacleType
    {
        None, Destructible, Industructible
    }

    [CreateAssetMenu(fileName = "Generic Obstacle", menuName = "Obstacle System/Generic Obstacle")]
    public class Obstacle : ScriptableObject
    {
        public ObstacleType obstacleType;
        public string obstacleName;
        public Transform obstacleTransform;
        public float spawnCooldown;
    }
}
