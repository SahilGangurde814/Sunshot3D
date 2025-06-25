using UnityEngine;

namespace Sunshot.Obstacles
{
    [CreateAssetMenu(fileName = "Indestructible Obstacle", menuName = "Obstacle System/Indestructible Obstacle")]
    public class IndestructibleObstacle : Obstacle
    {
        public IndestructibleObstacle()
        {
            obstacleType = ObstacleType.Industructible;
        }
    }
}
