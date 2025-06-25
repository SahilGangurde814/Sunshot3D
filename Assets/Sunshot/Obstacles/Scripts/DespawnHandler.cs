using Sunshot.CollectibleSystem;
using UnityEngine;

namespace Sunshot.Obstacles
{
    public class DespawnHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Return to obstacles pool.
            ObstacleController obstacleController = other.transform.GetComponent<ObstacleController>();
            if (obstacleController != null) 
                obstacleController.Deactivate();

            // Return to collectibles pool.
            CollectiblesHandler collectiblesHandler = other.GetComponent<CollectiblesHandler>();
            if(collectiblesHandler != null)
                collectiblesHandler.Deactivate();
        }
    }
}
