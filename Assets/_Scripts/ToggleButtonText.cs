using TMPro;
using UnityEngine;

public class ToggleButtonText : MonoBehaviour
{
    [SerializeField] private TMP_Text toggleButtonText;


    public void ToggleText()
    {
        if (toggleButtonText.text == "ON")
        {
            toggleButtonText.text = "OFF";
            toggleButtonText.transform.position += new Vector3(5f, 0, 0);
            toggleButtonText.color = new Color(0.28f, 0.28f, 0.28f, 1f); 
        }
        else
        {
            toggleButtonText.text = "ON";
            toggleButtonText.transform.position += new Vector3(-5f, 0, 0);
            toggleButtonText.color = new Color(0.044f, 0.33f, 0f, 1f); 
        }
    }


    
}
