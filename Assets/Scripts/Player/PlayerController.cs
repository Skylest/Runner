using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float speed = 12f; // Скорость движения персонажа
        private readonly float laneWidth = 3f; // Ширина каждой полосы движения
        private int currentLane = 0; // Текущая полоса (-1 - влево, 0 - по центру, 1 - вправо)
        private float speedRun = 12f;
        private float speedSprint = 15f;
        private float jumpForce = 50f;
        private bool isGrounded;
        private bool isDead = false;

        private SwipeInput swipeInput;
        private AnimatorController animator;
        private Rigidbody _rigidbody;
        private CapsuleCollider capsuleCollider;

        void Start()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            swipeInput = GetComponent<SwipeInput>();
            _rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<AnimatorController>();            
        }

        private void Update()
        {
            if (isDead)
                return;

            // Обработка ввода клавиш
            HandleKeyInput();

            MoveRun();
            MoveToLane();            
        }

        private void HandleKeyInput()
        {
            // Получаем входные данные от клавиатуры
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentLane = Mathf.Clamp(currentLane - 1, -1, 1);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.AnimateSprint();
                speed = speedSprint;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.AnimateRun();
                speed = speedRun;
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
                capsuleCollider.height = 1;
                capsuleCollider.center = new Vector3(0, 0.5f, 0);
                animator.Roll();
            }

            // Получаем данные о свайпе
            if (swipeInput.SwipedLeft)
                currentLane = Mathf.Clamp(currentLane - 1, -1, 1);
            if (swipeInput.SwipedRight)
                currentLane = Mathf.Clamp(currentLane + 1, -1, 1);
        }


        private void MoveToLane()
        {
            // Вычисляем новую позицию по горизонтали
            float targetX = currentLane * laneWidth;

            // Плавно перемещаем персонажа в сторону
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedSprint * Time.deltaTime);
        }

        private void MoveRun()
        {
            // Постоянное движение вперед
            Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;
            
            // Перемещение персонажа
            transform.Translate(forwardMovement, Space.World);     
        }

        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void ForceDown()
        {
            _rigidbody.AddForce(Vector3.down * jumpForce * 2, ForceMode.Impulse);
        }
        

        // Метод, вызываемый при контакте с землей (например, через OnCollisionEnter)
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.AnimateLand();
            }
            else 
            {
                speed = 0;
                animator.AnimateStay();
                isDead = true;
            }
        }

        // Метод, вызываемый при потере контакта с землей (например, через OnCollisionExit)
        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }

        public void OnRollEnd()
        {
            capsuleCollider.height = 2;
            capsuleCollider.center = new Vector3(0, 1, 0);
        }
    }
}
