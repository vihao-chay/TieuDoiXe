using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterManager : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text[] sentences;

    [Header("Timing")]
    public float charSpeed = 0.08f;
    public float holdTime = 1.5f;
    public float fadeDelay = 0.5f;

    void Start()
    {
        // 🔥 RESET TẤT CẢ TEXT TRƯỚC KHI BẮT ĐẦU
        foreach (var text in sentences)
        {
            if (text != null)
            {
                text.maxVisibleCharacters = 0;
                text.alpha = 0f;  // ẨN HOÀN TOÀN
                text.ForceMeshUpdate();
            }
        }
        StartCoroutine(TypeSequence());
    }

    IEnumerator TypeSequence()
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            TMP_Text text = sentences[i];
            if (text == null) continue;

            Debug.Log($"🔥 CÂU {i}: '{text.text}'");

            // 1. HIỆN TEXT + GÕ
            yield return StartCoroutine(TypeSentence(text));

            // 2. GIỮ
            yield return new WaitForSeconds(holdTime);

            // 3. ẨN HOÀN TOÀN TRƯỚC KHI SANG CÂU KHÁC
            yield return StartCoroutine(HideSentence(text));

            // 4. DELAY
            yield return new WaitForSeconds(fadeDelay);
        }
        OnIntroComplete();
    }

    IEnumerator TypeSentence(TMP_Text text)
    {
        // 🔥 BƯỚC 1: HIỆN TEXT
        CanvasGroup group = text.GetComponent<CanvasGroup>();
        if (group == null) group = text.gameObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;
        text.alpha = 1f;

        // 🔥 BƯỚC 2: RESET & GÕ
        text.maxVisibleCharacters = 0;
        text.ForceMeshUpdate();
        yield return null;  // Frame delay

        int totalChars = text.textInfo.characterCount;
        for (int j = 0; j < totalChars; j++)
        {
            text.maxVisibleCharacters = j + 1;
            yield return new WaitForSeconds(charSpeed);
        }
    }

    IEnumerator HideSentence(TMP_Text text)
    {
        // 🔥 ẨN HOÀN TOÀN: maxVisibleCharacters + alpha = 0
        text.maxVisibleCharacters = 0;
        CanvasGroup group = text.GetComponent<CanvasGroup>();
        if (group != null) group.alpha = 0f;
        text.alpha = 0f;
        text.ForceMeshUpdate();
        yield return null;
    }

    void OnIntroComplete()
    {
        Debug.Log("✅ MENU TIME!");
        var menuMgr = FindObjectOfType<MenuManager>();
        if (menuMgr != null)
        {
            menuMgr.introGroup.gameObject.SetActive(false);
            menuMgr.menuPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            foreach (var text in sentences)
            {
                if (text != null)
                {
                    text.maxVisibleCharacters = text.textInfo.characterCount;
                    text.alpha = 1f;
                }
            }
            OnIntroComplete();
        }
    }
}
