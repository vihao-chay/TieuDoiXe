using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;

    // CÔNG TẮC: Bật lên khi vào nhà, tắt đi khi ra ngoài
    public bool isFirstPerson = false;

    private Rigidbody rb;
    private Animator anim;
    private Transform cameraTransform;
    private bool isGrounded;

    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetMouseButton(1);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- PHÂN LOẠI GÓC NHÌN Ở ĐÂY ---
        if (isFirstPerson)
        {
            // 1. TRONG NHÀ (FPS): Đi tới/lui, trái/phải dựa theo hướng mặt đang nhìn
            moveDirection = transform.right * horizontal + transform.forward * vertical;

            // THÊM DÒNG NÀY ĐỂ CỨU NHÂN VẬT KHỎI BỊ LỌT ĐẤT
            moveDirection.y = 0f;

            moveDirection = moveDirection.normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                anim.SetFloat("Speed", isRunning ? 1f : 0.5f);
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
        else
        {
            // 2. NGOÀI TRỜI (TPS): Xoay cả người theo hướng Camera rồi mới đi
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                anim.SetFloat("Speed", isRunning ? 1f : 0.5f);
            }
            else
            {
                moveDirection = Vector3.zero;
                anim.SetFloat("Speed", 0f);
            }
        }

        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        // Dùng velocity thay vì MovePosition để di chuyển mượt mà và không lọt đất
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // GIỮ NGUYÊN lực hút trái đất (trục Y) để nhân vật luôn đứng vững trên mặt sàn
        targetVelocity.y = rb.linearVelocity.y;

        // Áp dụng lực chạy cho nhân vật
        rb.linearVelocity = targetVelocity;

        // Xử lý nhảy
        if (isJumping)
        {
            // Xóa bỏ lực rơi cũ trước khi nhảy để nhảy luôn chuẩn xác
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            isJumping = false;
        }
    }
}