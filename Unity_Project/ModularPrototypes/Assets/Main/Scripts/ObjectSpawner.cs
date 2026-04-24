using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject gameObject;
    [SerializeField] int rowCount;
    [SerializeField] int columnCount;
    [SerializeField] int depthCount;
    [SerializeField] int rowGap;
    [SerializeField] int columnGap;
    [SerializeField] int depthGap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                for (int z = 0; z < depthCount; z++)
                {
                    Vector3 position = new Vector3(j * columnGap, z * depthGap, i * rowGap);
                    Instantiate(gameObject, position, Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
