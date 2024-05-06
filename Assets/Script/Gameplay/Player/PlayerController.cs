using System.Collections;
using UnityEngine;

namespace RG.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CapsuleCollider thisCollider;

        [SerializeField] private float moveMultiplier = 2f;

        private bool isSliding;
        private int currentLane;
        private float slideDuration = 1f;
        private float slideElapcedTime = 0;
        private Coroutine moveToLaneCoroutinue;

        private int leftTriggerHash;
        private int rightTriggerHash;
        private int slideTriggerHash;
        private int runHash;


        private void Start()
        {
            currentLane = 0;

            leftTriggerHash = Animator.StringToHash("left");
            rightTriggerHash = Animator.StringToHash("right");
            slideTriggerHash = Animator.StringToHash("slide");
            runHash = Animator.StringToHash("run");
        }

        public void Run()
        {
            animator.SetBool(runHash, true);
        }

        public void Stop()
        {
            animator.SetBool(runHash, false);
        }

        private void Update()
        {
            if (GameManager.Instance.State != GameState.Playing) { return; }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentLane != -1)
                {
                    currentLane--;
                    if (!isSliding)
                    {
                        animator.ResetTrigger(rightTriggerHash);
                        animator.SetTrigger(leftTriggerHash);
                    }
                    ChangeLane();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentLane != 1)
                {
                    currentLane++;
                    if (!isSliding)
                    {
                        animator.ResetTrigger(leftTriggerHash);
                        animator.SetTrigger(rightTriggerHash);
                    }
                    ChangeLane();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
            {
                isSliding = true;
                animator.ResetTrigger(leftTriggerHash);
                animator.ResetTrigger(rightTriggerHash);
                animator.SetTrigger(slideTriggerHash);
                thisCollider.height = 60f;
                thisCollider.center = new Vector3(0, 30, 0);
            }

            if (isSliding)
            {
                slideElapcedTime += Time.deltaTime;
                if (slideElapcedTime >= slideDuration)
                {
                    slideElapcedTime = 0;
                    isSliding = false;
                    thisCollider.height = 116f;
                    thisCollider.center = new Vector3(0, 58.2f, 0);
                }
            }
        }

        private void ChangeLane()
        {
            if (moveToLaneCoroutinue != null)
            {
                StopCoroutine(moveToLaneCoroutinue);
            }

            switch (currentLane)
            {
                case 0:
                    moveToLaneCoroutinue = StartCoroutine(MoveToLane(Vector3.zero));
                    break;

                case -1:
                    moveToLaneCoroutinue = StartCoroutine(MoveToLane((Vector3.left * moveMultiplier)));
                    break;

                case 1:
                    moveToLaneCoroutinue = StartCoroutine(MoveToLane((Vector3.right * moveMultiplier)));
                    break;
            }
        }

        private IEnumerator MoveToLane(Vector3 targetPosition)
        {
            Vector3 startPosition = rb.position;
            float elapsedTime = 0f;
            float duration = 0.30f;

            while (elapsedTime < duration)
            {
                rb.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rb.position = targetPosition;

            animator.ResetTrigger(leftTriggerHash);
            animator.ResetTrigger(rightTriggerHash);
        }

        public void ResetPlayer()
        {
            currentLane = 0;
            isSliding = false;

            Stop();

            animator.ResetTrigger(leftTriggerHash);
            animator.ResetTrigger(rightTriggerHash);
            animator.ResetTrigger(slideTriggerHash);

            rb.position = Vector3.zero;

            gameObject.GetComponent<HealthController>().ResetHealth();
        }
    }

}