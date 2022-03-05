using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] tetrominos;

    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewTetromino()
    {
        Instantiate(tetrominos[Random.Range(0, tetrominos.Length)], transform.position, Quaternion.identity); // instantiate a tetromino (it will be randomly chosen depending on the length of tetromino available), at our position and with 0 rotation
    }
}
