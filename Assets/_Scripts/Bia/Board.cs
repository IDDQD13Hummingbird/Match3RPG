using UnityEngine;

public class Board : MonoBehaviour
{ public int width;
public int height;
public GameObject TilePrefab;
private Tile[,] allTiles;

public GameObject[] allowedIcons;
private int[,] tileTypes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allTiles = new Tile[width, height];
        tileTypes = new int[width, height];
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

                Tile tileComponent = tempTile.GetComponent<Tile>();
                allTiles[i,j]=tileComponent;

                int safeIconType = GetSafeIconType(i, j);
                tileTypes[i,j]=safeIconType;

                tileComponent.InitializeByType(safeIconType,allowedIcons);
            }
        }

    }

    private int GetSafeIconType(int x, int y)
    {
        for (int i = 0; i < allowedIcons.Length; i++)
        {
            if (IsSafeToCreate(x, y,i))
            {
                return i;
            }
        }

        return 0;
    }

    bool IsSafeToCreate(int x,int y,int Type)
    {
        if (x >= 2)    //horisontal check
        {
            if (tileTypes[x - 1, y] == Type && tileTypes[x - 2, y] == Type)
            {
                return false;
            }
        }

        if (y >= 2)    //horisontal check
        {
            if (tileTypes[x , y-1] == Type && tileTypes[x, y-2] == Type)
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
