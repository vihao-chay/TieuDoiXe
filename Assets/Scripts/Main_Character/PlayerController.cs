using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f; // Lực hút trái đất
    public float turnSmoothTime = 0.1f; // Độ mượt khi xoay người

    private CharacterController controller;
    private Animator anim;
    private Transform cam;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Tự động tìm Main Camera trong Scene
        cam = Camera.main.transform;
    }

    void Update()
    {
        // 1. Kiểm tra xem nhân vật có chạm đất không
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ép nhân vật bám sát mặt đất
        }

        // 2. Nhận lệnh AWSD
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Kiểm tra xem có giữ chuột phải không
        bool isRunning = Input.GetMouseButton(1);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // 3. Xử lý di chuyển
        if (direction.magnitude >= 0.1f)
        {
            // Tính toán góc xoay mặt dựa theo Camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Tiến lên phía trước
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // Kích hoạt Animation đi hoặc chạy
            anim.SetFloat("Speed", isRunning ? 1f : 0.5f);
        }
        else
        {
            // Đứng im
            anim.SetFloat("Speed", 0f);
        }

        // 4. Xử lý Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
        }

        // Áp dụng trọng lực để rớt xuống
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}