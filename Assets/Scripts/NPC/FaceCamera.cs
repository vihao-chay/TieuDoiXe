using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        // Tự động tìm Camera chính của game
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Ép vật thể (chữ) xoay mặt hướng y hệt như góc nhìn của Camera
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }
}