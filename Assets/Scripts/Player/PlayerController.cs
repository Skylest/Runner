using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float speed = 12f; // �������� �������� ���������
        private readonly float laneWidth = 3f; // ������ ������ ������ ��������
        private int currentLane = 0; // ������� ������ (-1 - �����, 0 - �� ������, 1 - ������)
        private float speedStrafe = 15f;
        private float jumpForce = 50f;
        private bool isGrounded;

        private SwipeInput swipeInput;
        private AnimatorController animator;
        private Rigidbody _rigidbody;

        void Start()
        {

            swipeInput = GetComponent<SwipeInput>();
            _rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<AnimatorController>();
        }

        private void Update()
        {
            // ��������� ����� ������
            HandleKeyInput();

            MoveRun();
            MoveToLane();
        }

        private void HandleKeyInput()
        {
            // �������� ������� ������ �� ����������
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentLane = Mathf.Clamp(currentLane - 1, -1, 1);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentLane = Mathf.Clamp(currentLane + 1, -1, 1);
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                animator.AnimateJump();
            }

            if (!isGrounded && Input.GetKeyDown(KeyCode.S))
            {
                ForceDown();
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.S))
            {
                ForceDown();
            }

            // �������� ������ � ������
            if (swipeInput.SwipedLeft)
                currentLane = Mathf.Clamp(currentLane - 1, -1, 1);
            if (swipeInput.SwipedRight)
                currentLane = Mathf.Clamp(currentLane + 1, -1, 1);
        }


        private void MoveToLane()
        {
            // ��������� ����� ������� �� �����������
            float targetX = currentLane * laneWidth;

            // ������ ���������� ��������� � �������
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedStrafe * Time.deltaTime);
        }

        private void MoveRun()
        {
            // ���������� �������� ������
            Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;
            
            // ����������� ���������
            transform.Translate(forwardMovement, Space.World);
            
            animator.AnimateRun();
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        private void ForceDown()
        {
            _rigidbody.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        }
        

        // �����, ���������� ��� �������� � ������ (��������, ����� OnCollisionEnter)
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.AnimateLand();
            }
        }

        // �����, ���������� ��� ������ �������� � ������ (��������, ����� OnCollisionExit)
        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}
