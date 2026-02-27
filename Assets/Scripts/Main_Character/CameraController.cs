using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cài đặt Mục tiêu")]
    public Transform target; // Vị trí nhân vật để camera nhìn theo
    public Vector3 offset = new Vector3(0f, 1.5f, 0f); // Nâng tâm nhìn lên cao một chút (ngang tầm ngực/đầu nhân vật)

    [Header("Cài đặt Camera")]
    public float distance = 4.0f; // Khoảng cách từ camera đến nhân vật
    public float mouseSensitivity = 3.0f; // Độ nhạy của chuột
    public float yMinLimit = -15f; // Giới hạn góc nhìn chúc xuống (tránh nhìn xuyên đất)
    public float yMaxLimit = 70f; // Giới hạn góc nhìn ngẩng lên

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Start()
    {
        // Khóa con trỏ chuột vào giữa màn hình và ẩn nó đi để chơi game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Dùng LateUpdate thay vì Update để đảm bảo Camera di chuyển SAU KHI nhân vật đã di chuyển xong (giúp camera không bị giật)
    void LateUpdate()
    {
        if (target == null) return;

        // Lấy thông tin khi bạn di chuyển chuột
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Giới hạn góc Y (lên/xuống) để camera không bị lật ngược vòng tròn
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);

        // Tính toán góc xoay
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Tính toán vị trí của camera lùi lại phía sau nhân vật
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 targetPosition = target.position + offset; // Vị trí nhân vật cộng thêm chiều cao

        // Áp dụng vị trí và góc xoay cho Camera
        transform.position = targetPosition + rotation * direction;
        transform.LookAt(targetPosition);
    }
}