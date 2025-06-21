using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
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
    

    void Start()
    {
        allTiles = new Tile[width, height];
        tileTypes = new int[width, height];
        allDots = new GameObject[width, height];
        // Center the board horizontally so all dots fit within the screen width
        float boardWorldWidth = width * 1f;
        float screenWorldWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
        float xOffset = (screenWorldWidth - boardWorldWidth) / 2f;
        Vector3 newPosition = transform.position;
        newPosition.x = xOffset;
        transform.position = newPosition;

        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition =
                    new Vector2(i + gameObject.transform.position.x, j + gameObject.transform.position.y);
                GameObject tempTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity);
                tempTile.transform.parent = this.transform;
                tempTile.name = "(" + i + ", " + j + ")";
                int dotToUse = Random.Range(0, dots.Length);
                int MaxIterations = 0;
                while (HasGeneratedMatchOnCreation(i, j, dots[dotToUse]) && MaxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    MaxIterations++;
                }

                MaxIterations = 0;
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + ")";
                allDots[i, j] = dot;
                //add destroy existing matches upon creation
            }
        }
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
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatch()
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

        yield return new WaitForSeconds(.3f);
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i + gameObject.transform.position.x,
                        j + gameObject.transform.position.y);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    piece.transform.parent = this.transform;
                    allDots[i, j] = piece;


                }
            }
        }

    }


    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.3f);
        while (CheckForExistingMatches())
        {
            yield return new WaitForSeconds(.3f);
            DestroyMatch();
        }


        for (int i = 0; i< allDots.GetLength(0); i++)
        {
            for (int j = 0; j < allDots.GetLength(1); j++)
            {
                allDots[i, j].GetComponent<Dot>().UpdateLastPosition();
            }
        }
    }
}
