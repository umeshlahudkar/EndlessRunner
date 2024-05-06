using UnityEngine;

namespace RG.Gameplay
{
    public class Coin : MonoBehaviour, IPickble
    {
        private bool hasCheckedForInsideObstacle = false;
        [SerializeField] private Transform thisTransform;

        public bool HasCheckedForInsideObstacle
        {
            get { return hasCheckedForInsideObstacle; }
            set { hasCheckedForInsideObstacle = value; }
        }

        public Transform ThisTransform
        {
            get { return thisTransform; }
        }

        public void Pick()
        {
            gameObject.SetActive(false);
        }

    }
}
