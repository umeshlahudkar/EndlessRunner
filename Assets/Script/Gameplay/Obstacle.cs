using UnityEngine;

namespace RG.Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private Transform thisTransform;
        [SerializeField] private ObstacleType obstacleType;

        public Transform ThisTransform
        {
            get { return thisTransform; }
        }

        public ObstacleType ObstacleType
        {
            get { return obstacleType; }
        }

        public void TakeDamage()
        {

        }
    }
}
