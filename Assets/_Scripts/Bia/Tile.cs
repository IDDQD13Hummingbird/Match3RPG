using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] icons;
    private int tileType = -1;

    void Start()
    {
        // Only initialize if not already initialized by Board
        //if (tileType == -1)
        //{
        //    Initialize();
        //}
    }

    void Update()
    {

    }

    //private void Initialize()
    //{
    //    int IconToUse = Random.Range(0, icons.Length);
    //    CreateIcon(IconToUse);
    //}

    // New method to be called by Board with a specific icon type
    //public void InitializeWithType(int iconType, GameObject[] iconArray)
    //{
    //    tileType = iconType;
    //    icons = iconArray; // Use the same icon array as Board
    //    CreateIcon(iconType);
    //}

    //private void CreateIcon(int iconType)
    //{
    //    if (iconType >= 0 && iconType < icons.Length)
    //    {
    //        GameObject icon = Instantiate(icons[iconType], transform.position, Quaternion.identity);
    //        icon.transform.parent = this.transform;
    //        icon.name = this.gameObject.name;
    //    }
    //}

    //// Getter for tile type
    //public int GetTileType()
    //{
    //    return tileType;
    //}

}
