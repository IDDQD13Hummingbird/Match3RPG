using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{
    // Falling animation support
    public bool isFalling = false;
    private float fallTargetY = 0f;
    private float fallSpeed;
    private float fallSpawnY = 0f; // Store the y position where the fall started

   
    private static float lastSwipeTime = -100f;
    private static float swipeCooldown = 1f;
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
    public float swipeResist = .1f;
    [Header("Dot Visuals")]
    public float dotScaleOverride = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float cellSize = 1f;

  

    private void Start()
    {
        fallSpeed = 10f; // Reset fall speed
        
    }

    // Call this to start a fall to a target Y position
    public void StartFalling(float targetY)
    {
        isFalling = true;
        fallTargetY = targetY;
        fallSpawnY = transform.position.y;
        // Set sprite alpha to 0 at start of fall
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }
    }


    public void Init(int col, int rw, float cSize, Board b)
    {
        column = col;
        row = rw;
        columnLast = col;
        rowLast = rw;
        x = col;
        y = rw;
        cellSize = cSize;
        board = b;
        // Only set position if not already falling (i.e., if not far above target)
        float targetY = rw * cellSize + board.transform.position.y;
        if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
        {
            transform.position = new Vector2(col * cellSize + board.transform.position.x, targetY);
        }
        // Dynamically scale the dot to leave a gap between dots (e.g., 10% gap)
        float scale = (dotScaleOverride > 0f) ? dotScaleOverride : cellSize * 0.9f; // Use override if set
        if (transform.localScale.x != scale || transform.localScale.y != scale)
        {
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        // Ensure collider matches the new visual size for interactability
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.size = new Vector2(scale, scale);
        collider.offset = Vector2.zero;
    }


    private void OnMouseDown()
    {
        // Block input if cooldown not finished or board is refilling
        if (Time.time - lastSwipeTime < swipeCooldown || Board.isRefilling)
            return;
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }


    private void OnMouseUp()
    {
        // Block input if cooldown not finished or board is refilling
        if (Time.time - lastSwipeTime < swipeCooldown || Board.isRefilling)
            return;
        lastTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            angle = Mathf.Atan2(lastTouchPosition.y - firstTouchPosition.y, lastTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            //Debug.Log(angle);
            MovePieces();
            lastSwipeTime = Time.time; // Set cooldown after a valid swipe
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Animate falling from above
        if (isFalling)
        {
            Vector3 pos = transform.position;
            // Lerp alpha from 0 to 1 based on progress from fallSpawnY to fallTargetY
            float totalDist = Mathf.Abs(fallSpawnY - fallTargetY);
            float progress = 1f;
            if (totalDist > 0.01f)
                progress = 1f - Mathf.Clamp01(Mathf.Abs(pos.y - fallTargetY) / totalDist);

            // Lerp alpha from 0 to 1
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = progress;
                sr.color = c;
            }

            pos.y = Mathf.MoveTowards(pos.y, fallTargetY, fallSpeed * Time.deltaTime);
            transform.position = pos;
            if (Mathf.Abs(pos.y - fallTargetY) < 0.01f)
            {
                pos.y = fallTargetY;
                transform.position = pos;
                isFalling = false;
                // Ensure alpha is 1 at end
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = 1f;
                    sr.color = c;
                }
            }
            return; // Don't do normal update logic while falling
        }
        FindMatch();
        if (isMatched)
        {
            SpriteRenderer newSprite = GetComponent<SpriteRenderer>();
            newSprite.color = Color.grey;
            

        }

        x = column;
        y = row;
        Vector2 targetPos = new Vector2(column * cellSize + board.transform.position.x, row * cellSize + board.transform.position.y);
        if ((Vector2)transform.position != targetPos)
        {
            // Halve the Lerp speed for slower animation
            transform.position = Vector2.Lerp(transform.position, targetPos, 0.5f);
        }
        else
        {
            transform.position = targetPos;
        }
        if (board.allDots[column, row] != this.gameObject)
        {
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
                UpdateLastPosition();
                // User-initiated match: start combo chain
                if (board != null) board.OnUserMatch();
                board.DestroyMatch();
            }
            otherDot = null;
        }
    }

    public void UpdateLastPosition()
    {
        rowLast = row;
        columnLast = column;
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
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = null;
            GameObject rightDot1 = null;
            try { leftDot1 = board.allDots[column - 1, row]; } catch { }
            try { rightDot1 = board.allDots[column + 1, row]; } catch { }
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
            GameObject upperDot1 = null;
            GameObject lowerDot1 = null;
            try { upperDot1 = board.allDots[column, row + 1]; } catch { }
            try { lowerDot1 = board.allDots[column, row - 1]; } catch { }
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
