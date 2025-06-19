using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;

    public float angle=0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp()
    {
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        angle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(angle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
