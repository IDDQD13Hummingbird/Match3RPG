using UnityEngine;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public static bool isRefilling = false;
    public int width;
    public int height;
    public GameObject TilePrefab;
    public GameObject[] possibleIcons; // Add this - assign the same icons array here
    public GameObject[,] allDots;
    private Tile[,] allTiles;
    private int[,] tileTypes; // Track what icon type each tile has
    public GameObject[] dots;
    public GameObject[] icons;
    private int tileType = -1;
    

    private float cellSize;
    private float lastScreenWidth = -1f;
    private float lastScreenHeight = -1f;
    private int score;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    private int comboCount = 0;
    private bool comboChainActive = false; // True if a combo chain is in progress
    private float comboTimer = 0f;
    private float comboDisplayDuration;
    private Vector3 comboTextBaseScale = Vector3.one;
    private float comboTextBounceTime = 0f;
    private float comboTextBounceDuration = 0.25f;
    private float comboTextBounceScale = 1.25f;
    private bool comboActive = false;

    //PARTICLES//
    public ParticleManager particle;

    void Start()
    {
        AdjustBoardToScreenAndRebuild();
        comboDisplayDuration = 1.5f;
        if (comboText != null)
        {
            comboTextBaseScale = comboText.rectTransform.localScale;
        }
        particle = GameObject.Find("ParticleManager").GetComponent<ParticleManager>(); //Particle manager needs to be in the scene
    }

    void Update()
    {
        float screenWorldWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        float screenWorldHeight = Camera.main.orthographicSize * 2f;
        if (Mathf.Abs(screenWorldWidth - lastScreenWidth) > 0.01f || Mathf.Abs(screenWorldHeight - lastScreenHeight) > 0.01f)
        {
            AdjustBoardToScreenAndRebuild();
        }

        // Combo text timer logic
        if (comboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                comboActive = false;
                if (comboText != null)
                {
                    Color c = comboText.color;
                    c.a = 0f;
                    comboText.color = c;
                }
            }
        }
        // Combo text bounce animation
        if (comboText != null && comboTextBounceTime > 0f)
        {
            comboTextBounceTime -= Time.deltaTime;
            float t = 1f - Mathf.Clamp01(comboTextBounceTime / comboTextBounceDuration);
            // Ease out bounce
            float scale = Mathf.Lerp(comboTextBounceScale, 1f, t) + Mathf.Sin(t * Mathf.PI) * 0.1f;
            comboText.rectTransform.localScale = comboTextBaseScale * scale;
            if (comboTextBounceTime <= 0f)
            {
                comboText.rectTransform.localScale = comboTextBaseScale;
            }
        }
    }

    private void AdjustBoardToScreenAndRebuild()
    {
        float screenWorldWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        float screenWorldHeight = Camera.main.orthographicSize * 2f;
        cellSize = Mathf.Min(screenWorldWidth / width, screenWorldHeight / height);
        float boardWorldWidth = width * cellSize;
        float boardWorldHeight = height * cellSize;
        float scale = 1f;
        if (boardWorldWidth > screenWorldWidth || boardWorldHeight > screenWorldHeight)
        {
            float scaleX = screenWorldWidth / boardWorldWidth;
            float scaleY = screenWorldHeight / boardWorldHeight;
            scale = Mathf.Min(scaleX, scaleY, 1f);
        }
        cellSize *= scale;
        boardWorldWidth = width * cellSize;
        boardWorldHeight = height * cellSize;
        // Add a row's worth of cellSize as padding to the left and right
        float horizontalPadding = cellSize * 1f; // 1 row's worth of padding on each side
        float xOffset = -screenWorldWidth / 2f + horizontalPadding + (screenWorldWidth - boardWorldWidth - 2 * horizontalPadding) / 2f + cellSize / 2f;
        float yOffset = -screenWorldHeight / 2f + (screenWorldHeight - boardWorldHeight) / 2f + cellSize / 2f;
        Vector3 newPosition = transform.position;
        newPosition.x = xOffset;
        newPosition.y = yOffset;
        transform.position = newPosition;
        lastScreenWidth = screenWorldWidth;
        lastScreenHeight = screenWorldHeight;

        // Destroy old dots if any
        if (allDots != null)
        {
            for (int i = 0; i < allDots.GetLength(0); i++)
            {
                for (int j = 0; j < allDots.GetLength(1); j++)
                {
                    if (allDots[i, j] != null)
                    {
                        Destroy(allDots[i, j]);
                    }
                }
            }
        }
        allTiles = new Tile[width, height];
        tileTypes = new int[width, height];
        allDots = new GameObject[width, height];
        CreateBoard(cellSize);
    }
    

    private void CreateBoard(float cellSize)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i * cellSize + gameObject.transform.position.x, j * cellSize + gameObject.transform.position.y);
                GameObject tempTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity);
                tempTile.transform.parent = this.transform;
                tempTile.name = "(" + i + ", " + j + ")";
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;
                // Ensure no more than 2 in a row or column
                while ((HasGeneratedMatchOnCreation(i, j, dots[dotToUse]) || CausesLongerMatch(i, j, dots[dotToUse])) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                }

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + ")";
                Dot dotScript = dot.GetComponent<Dot>();
                if (dotScript != null)
                {
                    dotScript.Init(i, j, cellSize, this);
                }
                allDots[i, j] = dot;
            }
        }
    }

    // Prevents more than 2 in a row/column (no 3+ at start)
    private bool CausesLongerMatch(int column, int row, GameObject piece)
    {
        string tag = piece.tag;
        // Check for more than 2 in a row horizontally
        int count = 1;
        for (int k = 1; k <= 2; k++)
        {
            int c = column - k;
            if (c >= 0 && allDots[c, row] != null && allDots[c, row].tag == tag)
                count++;
            else
                break;
        }
        if (count >= 3) return true;

        // Check for more than 2 in a column vertically
        count = 1;
        for (int k = 1; k <= 2; k++)
        {
            int r = row - k;
            if (r >= 0 && allDots[column, r] != null && allDots[column, r].tag == tag)
                count++;
            else
                break;
        }
        if (count >= 3) return true;

        return false;
    }

    private bool HasGeneratedMatchOnCreation(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
            {
                return true;
            }
            else if (column <= 1 || row <= 1)
            {
                if (row > 1)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }

                if (column > 1)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void DestroyMatchAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            SpawnEffect(column + gameObject.transform.position.x,row + gameObject.transform.position.y); //vfx

            // Add score for each dot destroyed
            score += 10;
            if (scoreText != null)
                scoreText.text = "Score: " + score;
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    // Call this when a user makes a manual match (swipe)
    public void OnUserMatch()
    {
        comboCount = 1;
        comboChainActive = true;
        ShowComboText();
    }

    // Call this for every chain match (auto match from falling dots)
    public void OnChainCombo()
    {
        if (!comboChainActive)
        {
            comboCount = 1;
            comboChainActive = true;
        }
        else
        {
            comboCount++;
        }
        ShowComboText();
    }

    private void ShowComboText()
    {
        if (comboText != null && comboCount > 1)
        {
            Debug.Log($"COMBO x{comboCount}!");
            comboText.text = $"COMBO x{comboCount}";
            Color c = comboText.color;
            c.a = 1f;
            comboText.color = c;
            comboActive = true;
            comboTimer = comboDisplayDuration;
            // Bounce effect
            comboTextBounceTime = comboTextBounceDuration;
        }
        else if (comboText != null)
        {
            Color c = comboText.color;
            c.a = 0f;
            comboText.color = c;
        }
    }

    public void DestroyMatch(bool isChainCombo = false)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchAt(i, j);
                }
            }
        }

        if (isChainCombo)
        {
            OnChainCombo();
        }
        StartCoroutine(ClashRowsCo());
    }

    private IEnumerator ClashRowsCo() //fill the empty space on the board after the tiles are destroyed
    {
        int emptySpaceCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    emptySpaceCount++;
                }
                else if (emptySpaceCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= emptySpaceCount;
                    allDots[i, j] = null;
                }
            }

            emptySpaceCount = 0;
        }

        yield return new WaitForSeconds(.01f);
        StartCoroutine(FillBoardCo());

    }

    private bool CheckForExistingMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }

        }

        return false;
    }

    private void RefillBoard()
    {
        // Use the board's cellSize field directly
        float fallStartYOffset = 4f; // How high above the board new dots spawn
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 spawnPosition = new Vector2(i * cellSize + gameObject.transform.position.x, (j + fallStartYOffset) * cellSize + gameObject.transform.position.y);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], spawnPosition, Quaternion.identity);
                    piece.transform.parent = this.transform;
                    Dot dotScript = piece.GetComponent<Dot>();
                    if (dotScript != null)
                    {
                        dotScript.Init(i, j, cellSize, this);
                        dotScript.StartFalling(j * cellSize + gameObject.transform.position.y);
                    }
                    allDots[i, j] = piece;
                }
            }
        }
    }


    private IEnumerator FillBoardCo()
    {
        isRefilling = true;
        RefillBoard();
        // Wait for all falling dots to finish
        bool anyFalling;
        do
        {
            anyFalling = false;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (allDots[i, j] != null)
                    {
                        Dot dot = allDots[i, j].GetComponent<Dot>();
                        if (dot != null && dot.isFalling)
                        {
                            anyFalling = true;
                        }
                    }
                }
            }
            if (anyFalling)
                yield return null;
        } while (anyFalling);

        yield return new WaitForSeconds(.1f);
        bool firstMatch = true;
        bool userInputMatch = false;
        while (CheckForExistingMatches())
        {
            yield return new WaitForSeconds(.3f);
            DestroyMatch(!firstMatch ? true : false);
            if (firstMatch && !userInputMatch)
            {
                OnChainCombo(); // Trigger combo for first auto-match after refill
            }
            if (firstMatch) userInputMatch = true;
            firstMatch = false;
            // Wait for new falls
            do
            {
                anyFalling = false;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (allDots[i, j] != null)
                        {
                            Dot dot = allDots[i, j].GetComponent<Dot>();
                            if (dot != null && dot.isFalling)
                            {
                                anyFalling = true;
                            }
                        }
                    }
                }
                if (anyFalling)
                    yield return null;
            } while (anyFalling);
        }

        for (int i = 0; i < allDots.GetLength(0); i++)
        {
            for (int j = 0; j < allDots.GetLength(1); j++)
            {
                if (allDots[i, j] != null)
                {
                    allDots[i, j].GetComponent<Dot>().UpdateLastPosition();
                }
            }
        }
        isRefilling = false;
        // Combo chain ends only on new user input, not after refill
        // (Do not reset comboCount here)
    }

    /*Effects*/
    void SpawnEffect(float x, float y)
    {
        //spawn particles effect 
        particle.pool[0].transform.position = new Vector3(x, y, -1);
        particle.pool[0].GetComponent<ParticleSystem>().Emit(1);
        //
    }
}
