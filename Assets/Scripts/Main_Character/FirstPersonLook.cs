using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float mouseSensitivity = 3f; // Độ nhạy của chuột
    public Transform playerBody; // Khung thân nhân vật để xoay trái/phải

    private float xRotation = 0f;

    void Update()
    {
        // Nhận dữ liệu khi người chơi trượt chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Tính toán góc ngẩng lên/cúi xuống (trục X của Camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Giới hạn góc từ -80 đến 80 độ để không bị lộn vòng ra đằng sau

        // Xoay Camera (chỉ xoay cái đầu lên xuống)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay toàn bộ thân nhân vật sang trái/phải (trục Y)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}