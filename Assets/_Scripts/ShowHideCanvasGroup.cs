using UnityEngine;

public class ShowHideCanvasGroup : MonoBehaviour
{
    [SerializeField] private bool startVisible = false;
    private void Start()
    {
        if (startVisible)
        {
            ShowCurrentCanvasGroup();
        }
        else
        {
            HideCurrentCanvasGroup();
        }
    }

    public void HideCurrentCanvasGroup()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void ShowCurrentCanvasGroup()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
