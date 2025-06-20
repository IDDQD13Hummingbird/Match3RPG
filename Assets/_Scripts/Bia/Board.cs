using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject TilePrefab;
    public GameObject[] possibleIcons; // Add this - assign the same icons array here
    public GameObject[,] allDots;
    private Tile[,] allTiles;
    private int[,] tileTypes; // Track what icon type each tile has

    public GameObject[] icons;
    private int tileType = -1;

    void Start()
    {
        allTiles = new Tile[width, height];
        tileTypes = new int[width, height];
        allDots = new GameObject[width, height];
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i+gameObject.transform.position.x, j+gameObject.transform.position.y);   
                GameObject tempTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity);
                tempTile.transform.parent = this.transform;
                tempTile.name = "(" + i + ", " + j + ")";

                int IconToUse = Random.Range(0, icons.Length);
                //CreateIcon(IconToUse, tempPosition);

                // Get the Tile component and assign a safe icon
                Tile tileComponent = tempTile.GetComponent<Tile>();
                allTiles[i, j] = tileComponent;

                // Find a safe icon type that won't create matches
                int safeIconType = GetSafeIconType(i, j);
                tileTypes[i, j] = safeIconType;

                // Initialize the tile with the safe icon type
                allDots[i, j] = InitializeWithType(safeIconType, possibleIcons, tempPosition, i, j);
                //allDots[i, j] = InitializeWithType(safeIconType, possibleIcons, tempPosition);
                //allDots[j, i].name = "(" + i + ", " + j + ")"; 
            }
        }
    }


    public GameObject InitializeWithType(int iconType, GameObject[] iconArray, Vector3 Position, int i, int j)
    {
        tileType = iconType;
        icons = iconArray; // Use the same icon array as Board
        GameObject temp=CreateIcon(iconType, Position);
        temp.name = "(" + i + ", " + j + ")";
        return temp;
    }

    private GameObject CreateIcon(int iconType, Vector3 Position)
    {
        if (iconType >= 0 && iconType < icons.Length)
        {
            GameObject icon = Instantiate(icons[iconType], Position, Quaternion.identity);
            icon.transform.parent = this.transform;
            icon.name = this.gameObject.name;
            return icon;
        }
        else
        {
            return null;
        }
            

    }

    private int GetSafeIconType(int x, int y)
    {
        //// Try each possible icon type
        //for (int i = 0; i < 4; i++)
        //{
        //    if (IsSafeToPlace(x, y, i))
        //    {
        //        return i;
        //    }
        //}

        while (true)
        {
            int i = Random.Range(0, 4);

            if (IsSafeToPlace(x, y, i))
            {
                return i;
                break;
            }
        }

        // Fallback - this shouldn't happen with enough icon types
        return 0;
    }

    private bool IsSafeToPlace(int x, int y, int iconType)
    {
        // Check horizontal matches (left side)
        if (x >= 2)
        {
            if (tileTypes[x - 1, y] == iconType && tileTypes[x - 2, y] == iconType)
            {
                return false; // Would create a horizontal match
            }
        }

        // Check vertical matches (bottom side)
        if (y >= 2)
        {
            if (tileTypes[x, y - 1] == iconType && tileTypes[x, y - 2] == iconType)
            {
                return false; // Would create a vertical match
            }
        }

        return true; // Safe to place
    }
}
