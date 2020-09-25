using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{

    public GameObject particlePrefab;
    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 25; i++)
            Instantiate(particlePrefab, new Vector3(583.25f + (float)random.NextDouble() * 0.78f - .354f, -5 + (float)random.NextDouble() * 5.6f, 511 + (float)random.NextDouble() * 0.78f - .354f), Quaternion.identity);
    }
}
