using UnityEngine;
using UnityEngine.UI;

public class FlipFlopButtonSprites : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite[] standardSprite;
    [SerializeField] private Sprite[] standardSpritePressed;
    private Image imageComponent;
    private Button buttonComponent;
    private bool isFlipped = false;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();
    }

    public void FlipFlop()
    {
        isFlipped = !isFlipped;

        SetButtonSprites(isFlipped);
    }

    private void SetButtonSprites(bool firstSprites)
    {
        if (firstSprites)
        {
            // Use the first set of sprites
            if (standardSprite != null && standardSprite.Length > 0)
                imageComponent.sprite = standardSprite[0];

            if (standardSpritePressed != null && standardSpritePressed.Length > 0)
            {
                SpriteState spriteState = buttonComponent.spriteState;
                spriteState.pressedSprite = standardSpritePressed[0];
                buttonComponent.spriteState = spriteState;
            }
        }
        else
        {
            // Use the second set of sprites (if available)
            if (standardSprite != null && standardSprite.Length > 1)
                imageComponent.sprite = standardSprite[1];

            if (standardSpritePressed != null && standardSpritePressed.Length > 1)
            {
                SpriteState spriteState = buttonComponent.spriteState;
                spriteState.pressedSprite = standardSpritePressed[1];
                buttonComponent.spriteState = spriteState;
            }
        }
    }
}
