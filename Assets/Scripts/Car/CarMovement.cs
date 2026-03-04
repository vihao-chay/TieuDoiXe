using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float boostSpeed = 10f;
    public float turnSpeed = 50f;

    [Header("Camera Cố Định")]
    public Transform carCamera; // Kéo CarCamera vào đây

    public Vector3 centerOfMassOffset = new Vector3(0f, -1f, 0f);
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ép trọng tâm của xe lún xuống gầm (-1 trục Y) để xe không bao giờ lộn nhào
        rb.centerOfMass = centerOfMassOffset;
    }

    // ĐÃ XÓA HÀM UPDATE() CHỨA CODE XOAY CHUỘT

    void FixedUpdate()
    {
        // CƠ CHẾ LÁI XE BẰNG WASD
        float vertical = Input.GetAxis("Vertical"); // Phím W, S
        float horizontal = Input.GetAxis("Horizontal"); // Phím A, D

        // Nhấn Shift hoặc Chuột phải để tăng tốc
        bool isBoosting = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;

        // Tính toán lực chạy tới/lui
        Vector3 moveDirection = transform.forward * vertical * currentSpeed;
        moveDirection.y = rb.linearVelocity.y; // Giữ nguyên trọng lực rơi xuống đất

        rb.linearVelocity = moveDirection;

        // Xoay xe (chỉ cho phép xoay vô lăng khi xe đang di chuyển)
        if (vertical != 0)
        {
            float turnMultiplier = vertical > 0 ? 1f : -1f; // Đi lùi thì đảo ngược góc lái
            Quaternion turnRotation = Quaternion.Euler(0f, horizontal * turnSpeed * turnMultiplier * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}