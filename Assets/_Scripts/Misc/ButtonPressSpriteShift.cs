using UnityEngine;
using UnityEngine.UI;

public class ButtonPressSpriteShift : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    public void PressDown()
    {
        targetImage.transform.position += new Vector3(0, -0.8f, 0);
        targetImage.color = new Color(0.6f, 0.6f, 0.6f, 1f); // Slightly dim the color
    }

    public void PressUp()
    {
        targetImage.transform.position += new Vector3(0, 0.8f, 0);
        targetImage.color = new Color(1f, 1f, 1f, 1f); // Reset color
    }

}
