using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    public int column;
    public int row;
    public int x;
    public int y;
    private GameObject otherDot;
    private Board board;
    private Vector2 tempPosition;
    public float angle=0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindObjectOfType<Board>(); // only works if we have 1 board
        x = (int)transform.localPosition.x;
        y = (int)transform.localPosition.y;
        row = y;
        column = x;
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
        MovePieces();
    }

    // Update is called once per frame
    void Update()
    {
        x = column;
        y = row;
        if (Mathf.Abs(x-transform.localPosition.x)> .1){
            //move
            tempPosition=new Vector2(x, transform.localPosition.y);
            transform.localPosition = Vector2.Lerp(transform.localPosition,tempPosition, .4f );
        }
        else
        {
            tempPosition = new Vector2(x, transform.localPosition.y);
            transform.localPosition = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(y - transform.localPosition.y) > .1)
        {
            //move
            tempPosition = new Vector2( transform.localPosition.x, y);
            transform.localPosition = Vector2.Lerp(transform.localPosition, tempPosition, .4f);
        }
        else
        {
            tempPosition = new Vector2(transform.localPosition.x, y);
            transform.localPosition = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    void MovePieces()
    {
        if(angle>= -45 && angle <= 45 && column < board.width)
        { //move right
            otherDot = board.allDots[column + 1, row]; //
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (angle > 45 && angle <= 135 && row < board.height)
        { //move up
            otherDot = board.allDots[column, row+1]; //
            otherDot.GetComponent<Dot>().row-= 1;
            row += 1;
        }
        else if (angle > 135 || angle <= -135 && column >0)
        { //move left
            otherDot = board.allDots[column - 1, row]; //
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (angle < -45 && angle >= -135 && row >0)
        { //move down
            otherDot = board.allDots[column, row-1]; //
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
    }

}
