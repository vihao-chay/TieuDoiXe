using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;

    // Thêm biến này để gọi tới nhân vật
    public PlayerController playerMovement;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thirdPersonCamera.SetActive(false);
            firstPersonCamera.SetActive(true);

            // Bật công tắc FPS cho nhân vật
            playerMovement.isFirstPerson = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);

            // Tắt công tắc FPS, trả về TPS
            playerMovement.isFirstPerson = false;
        }
    }
}