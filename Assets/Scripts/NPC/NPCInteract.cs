using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject promptUI; // Chữ Ấn F của bạn
    public DialogueManager dialogueManager;

    [Header("Góc Quay & Dữ Liệu")]
    public string npcName = "Cán bộ";
    public GameObject dialogueCamera; // Camera quay qua vai
    [TextArea(3, 10)]
    public string[] sentences;

    private bool playerInRange = false;
    private GameObject playerRef;

    void Start()
    {
        promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            promptUI.SetActive(false);
            // Bắt đầu nói chuyện
            dialogueManager.StartDialogue(npcName, sentences, dialogueCamera, playerRef);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerRef = other.gameObject;
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerRef = null;
            promptUI.SetActive(false);
        }
    }
}