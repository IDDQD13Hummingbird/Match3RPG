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
            toggleButtonText.color = new Color(1f, .5f, 0.2f, 1f); // Change color to red
        }
        else
        {
            toggleButtonText.text = "ON";
            toggleButtonText.transform.position += new Vector3(-5f, 0, 0);
        }
    }


    
}
