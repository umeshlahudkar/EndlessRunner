using UnityEngine;

namespace RG.Gameplay
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private HealthController healthController;

        private void OnTriggerEnter(Collider other)
        {
            IPickble pickble = other.gameObject.GetComponent<IPickble>();
            if (pickble != null)
            {
                pickble.Pick();
                CoinManager.Instance.AddCoins();
                return;
            }


            Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.TakeDamage();
                healthController.TakeDamage();
                GameManager.Instance.ResetLevelSpeed();
            }

        }
    }
}
