using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonClickBG : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("BG References")]
    public Image bgPressed;

    private Vector3 hoverScale = Vector3.one;
    private Vector3 pressedScale = Vector3.one * 1.2f;
    private bool isHovered = false;
    private bool isPressed = false;  // 🔥 THÊM NÀY!

    void Start()
    {
        if (bgPressed != null)
        {
            bgPressed.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (bgPressed != null && !isPressed)
        {  // 🔥 Chỉ hiện nếu KHÔNG đang nhấn
            bgPressed.gameObject.SetActive(true);
            bgPressed.transform.localScale = hoverScale;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (bgPressed != null)
        {
            // 🔥 LUÔN ẨN khi hover ra, BẤT KỂ đang nhấn hay không
            bgPressed.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;  // 🔥 Track trạng thái nhấn

        if (bgPressed != null && isHovered)
        {
            bgPressed.gameObject.SetActive(true);  // Đảm bảo hiện
            bgPressed.transform.localScale = pressedScale;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;  // 🔥 Reset

        if (bgPressed != null && isHovered)
        {
            bgPressed.transform.localScale = hoverScale;  // Về hover size
        }
    }
}
