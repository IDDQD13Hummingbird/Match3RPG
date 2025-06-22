using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject ParticleEffect;
    public GameObject[] pool;
    public int poolSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i]=Instantiate(ParticleEffect, new Vector3(-30, 0, -3), Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
