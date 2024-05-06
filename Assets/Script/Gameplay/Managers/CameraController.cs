using UnityEngine;

namespace RG.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform thistransform;
        [SerializeField] private Transform target;
        [SerializeField] private float smoothSpeed = 0.125f;
        private Vector3 originalPosition;

        private void Start()
        {
            if (target != null)
            {
                originalPosition = thistransform.position;
            }
        }

        void LateUpdate()
        {
            if (GameManager.Instance.State != GameState.Playing) { return; }

            if (target != null)
            {
                Vector3 desiredPosition = target.position;
                desiredPosition.y = originalPosition.y;
                Vector3 smoothedPosition = Vector3.Lerp(thistransform.position, desiredPosition, smoothSpeed);
                thistransform.position = smoothedPosition;
            }
        }
    }
}
