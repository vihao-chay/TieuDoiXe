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

    // Các biến lưu trữ
    private GameObject gameplayCamera;
    private GameObject currentDialogueCamera;
    private GameObject currentPlayer; // Biến mới: Dùng để nhớ và giấu nhân vật

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

        dialoguePanel.SetActive(true);
        acceptButton.SetActive(false);

        // Đổi máy quay
        currentDialogueCamera = npcCamera;
        if (gameplayCamera != null) gameplayCamera.SetActive(false);
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(true);

        // MỚI: Tàng hình nhân vật chính thay vì khóa di chuyển
        currentPlayer = player;
        if (currentPlayer != null) currentPlayer.SetActive(false);

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
            acceptButton.SetActive(true);
        }
    }

    public void AcceptQuest()
    {
        dialoguePanel.SetActive(false);

        // Trả lại máy quay
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.SetActive(true);

        // MỚI: Hiện lại nhân vật chính
        if (currentPlayer != null) currentPlayer.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}