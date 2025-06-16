using UnityEngine;

public class Board : MonoBehaviour
{ public int width;
public int height;
public GameObject TilePrefab;
private Tile[,] allTiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allTiles = new Tile[width, height];
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject tempTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity);
                tempTile.transform.parent = this.transform;
                tempTile.name = "(" + i + ", " + j + ")";
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
