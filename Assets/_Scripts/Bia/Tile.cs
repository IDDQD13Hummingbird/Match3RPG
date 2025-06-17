using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] icons;
    private int tileType=-1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tileType == -1)
        {
            Initialize();
        }
    }

    public int getTileType()
    {
        return tileType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        int IconToUse = Random.Range(0, icons.Length);
        GameObject icon = Instantiate(icons[IconToUse], transform.position, Quaternion.identity);
        //icon.transform.parent = this.transform;
        //icon.name = this.gameObject.name;
    }

    public void InitializeByType(int Type, GameObject[] IconsArray)
    {
        tileType = Type;
        icons = IconsArray;
        CreateIcon(Type);
    }

    private void CreateIcon(int Type)
    {
        //int IconToUse = Random.Range(0, icons.Length);
        if (Type >= 0 && Type < icons.Length)
        {
            GameObject icon = Instantiate(icons[Type], transform.position, Quaternion.identity);
            icon.transform.parent = this.transform;
            icon.name = this.gameObject.name;
        }  
    }
}
