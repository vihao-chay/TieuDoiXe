using UnityEngine;

public class NPCEndInteract : MonoBehaviour
{
    public GameObject promptUI; // Chữ Ấn F
    public DialogueManager dialogueManager;

    [Header("Góc Quay & Dữ Liệu Của NPC Cuối")]
    public string npcName = "Chỉ huy chiến dịch";

    // Đã đổi tên biến từ dialogueCamera thành endCamera để ép Unity reset ô kéo thả
    public GameObject endCamera;

    [TextArea(3, 10)]
    public string[] sentences;

    private bool playerInRange = false;
    private GameObject playerRef;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (promptUI != null) promptUI.SetActive(false);
            // Bắt đầu nói chuyện và truyền góc quay MỚI vào
            dialogueManager.StartDialogue(npcName, sentences, endCamera, playerRef);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerRef = other.gameObject;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerRef = null;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }
}