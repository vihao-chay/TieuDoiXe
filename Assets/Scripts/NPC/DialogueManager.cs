using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Giao Diện UI (Kéo thả vào đây)")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject acceptButton;

    [Header("Cài đặt")]
    public float typingSpeed = 0.04f;

    private string[] currentSentences;
    private int index;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Các biến lưu trữ để trả lại trạng thái cũ
    private GameObject gameplayCamera;
    private GameObject currentDialogueCamera;
    private MonoBehaviour playerController;

    void Start()
    {
        dialoguePanel.SetActive(false);
        acceptButton.SetActive(false);
        gameplayCamera = Camera.main.gameObject;
    }

    void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            // Bấm Space hoặc Click chuột trái để chạy chữ nhanh
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    dialogueText.text = currentSentences[index];
                    isTyping = false;
                }
                else
                {
                    NextSentence();
                }
            }
        }
    }

    public void StartDialogue(string npcName, string[] sentences, GameObject npcCamera, GameObject player)
    {
        nameText.text = npcName;
        currentSentences = sentences;
        index = 0;

        // Hiện UI
        dialoguePanel.SetActive(true);
        acceptButton.SetActive(false);

        // Đổi góc máy quay phim
        currentDialogueCamera = npcCamera;
        if (gameplayCamera != null) gameplayCamera.SetActive(false);
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(true);

        // Khóa Kim Đồng đứng im
        playerController = player.GetComponent<PlayerController>();
        if (playerController != null) playerController.enabled = false;

        // Hiện chuột lên để bấm nút
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in currentSentences[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextSentence()
    {
        if (index < currentSentences.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            // Đã nói xong -> Hiện nút
            acceptButton.SetActive(true);
        }
    }

    // Hàm gọi khi bấm nút Accept
    public void AcceptQuest()
    {
        dialoguePanel.SetActive(false);

        // Trả lại máy quay
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.SetActive(true);

        // Mở khóa cho nhân vật đi tiếp
        if (playerController != null) playerController.enabled = true;

        // Giấu chuột đi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}