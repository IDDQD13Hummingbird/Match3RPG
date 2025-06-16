using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] icons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        int IconToUse = Random.Range(0, icons.Length);
        GameObject icon = Instantiate(icons[IconToUse], transform.position, Quaternion.identity);
        icon.transform.parent = this.transform;
        icon.name = this.gameObject.name;
    }
}
