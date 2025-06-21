using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    [Header("Tiles Board Variables")]
    private Vector2 firstTouchPosition;
    private Vector2 lastTouchPosition;
    public int column;
    public int row;
    public int columnLast;
    public int rowLast;
    public int x;
    public int y;
    public bool isMatched = false;
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
        rowLast = row;
        columnLast = column;
        

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
        FindMatch();
        if (isMatched)
        {
            SpriteRenderer newSprite = GetComponent<SpriteRenderer>();
            newSprite.color = Color.grey;  //new Color(0f, 0f, of, .2f);

        }
       

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

    //Co-routine
    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = rowLast;
                column = columnLast;

            }
            else
            {
               
                rowLast=row;
                columnLast=column;
            }
                otherDot = null;
        }
    }

    void MovePieces()
    {
        if(angle>= -45 && angle <= 45 && column < board.width-1)
        { //move right
            otherDot = board.allDots[column + 1, row]; //
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (angle > 45 && angle <= 135 && row < board.height-1)
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
        StartCoroutine(CheckMoveCo());
        
    }



    void FindMatch()
    {
        if(column>0 && column < board.width - 1)
        {
            GameObject leftDot1=board.allDots[column-1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null && leftDot1 != this.gameObject && rightDot1 != this.gameObject)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
            
        }
        if (row > 0 && row < board.height - 1) 
        {
            GameObject upperDot1 = board.allDots[column , row+1];
            GameObject lowerDot1 = board.allDots[column, row-1];
            if (upperDot1 != null && lowerDot1 != null && upperDot1 != this.gameObject && lowerDot1 != this.gameObject)
            {
                if (upperDot1.tag == this.gameObject.tag && lowerDot1.tag == this.gameObject.tag)
                {
                    upperDot1.GetComponent<Dot>().isMatched = true;
                    lowerDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
               
        }
    }
}
