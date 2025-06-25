using UnityEngine;

namespace Sunshot.Obstacles
{
    [CreateAssetMenu(fileName = "Destructible Obstacle", menuName = "Obstacle System/Destructible Obstacle")]
    public class DestrutibleObstacle : Obstacle
    {
        public DestrutibleObstacle()
        {
            obstacleType = ObstacleType.Destructible;
        }
    }
}
