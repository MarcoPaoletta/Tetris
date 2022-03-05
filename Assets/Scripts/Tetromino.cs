using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 0.9f;
    private float pastTime;

    public static  int width = 30;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height]; // create a grid with the width of 10 and the height of 20

    public Vector3 rotationPoint;

    public static int score = 0;
    public static int difficult = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) // if we press the left arrow
        {
            transform.position -= new Vector3(1, 0, 0); // substract 1 to the x index so that it will move to the left
            if (!limits()) // if we are on a limit
            {
                transform.position += new Vector3(1, 0, 0); // sum 1 to the x index so that it will move to the right
            }
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) // if we press the right arrow
        {
            transform.position += new Vector3(1, 0, 0); // sum 1 to the x index so that it will move to the right
            if (!limits()) // if we are on a limit
            {
                transform.position -= new Vector3(1, 0, 0); // substract 1 to the x index so that it will move to the left
            }
        }

        if (Time.time - pastTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 20: fallTime)) // if the current time minus the past time is grater than the fall time an we press the down key, we will move down faster. If we do not press the down key, wi will move down normally
        {
            transform.position -= new Vector3(0, 1, 0); // substract 1 to the y index so that it will move down
            if (!limits()) // if we are on a limit
            {
                transform.position += new Vector3(0, 1, 0); // increase 1 to the y index so that it will move up
                AddToGrid();
                CheckLines();
                this.enabled = false; // disable the current piece so that we will not be able to move it anymore
                FindObjectOfType<Generator>().NewTetromino(); // create a new tetromino
            }

            pastTime = Time.time; // update the past time
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) // if we press the up arrow
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!limits())
            {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }

        IncreaseLevel();
        IncreaseDifficult();
    }

    bool limits() 
    {
        foreach(Transform child in transform) // obtain every child
        {
            int xInt = Mathf.RoundToInt(child.transform.position.x);
            int yInt = Mathf.RoundToInt(child.transform.position.y);

            if (xInt < 0 || xInt >= width || yInt < 0 || yInt >= height) // if we are on a limit either left, right, bottom
            {
                return false;
            }

            if (grid[xInt, yInt] != null) // if we collide with other piece
            {
                return false;
            }
        }

        return true; // if we do not collide with anything 
    }

    void AddToGrid()
    {
        foreach(Transform child in transform) // obtain every child
        {
            int xInt = Mathf.RoundToInt(child.transform.position.x);
            int yInt = Mathf.RoundToInt(child.transform.position.y);

            grid[xInt, yInt] = child; // add the positions of the child

            if(yInt >= 19)
            {
                score = 0;
                difficult = 0;
                fallTime = 0.8f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void CheckLines()
    {
        for(int i = height - 1; i >= 0; i--) // iterate between every line 
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                LowerLine(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++) // iterate between every column
        {
            if(grid[j, i] == null) // if there is not line created
            {
                return false;
            }
        }
        score += 100;
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++) // iterate between every column
        {
            // delete the line
            Destroy(grid[j, i].gameObject); 
            grid[j, i] = null;
        }
    }

    void LowerLine(int i)
    {
        for(int y = i; y < height; y++) // iterate between every line 
        {
            for(int j = 0; j < width; j++) // iterate between every column
            {
                if(grid[j, y] != null) // if there is a line
                {
                    // lower the line which is up
                    grid[j, y - 1] = grid[j, y]; 
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    } 

    void IncreaseLevel()
    {
        // match in godot
        switch(score)  
        {
            case 200:
                difficult = 1;
                break;
            case 400:
                difficult = 2;
                break;
        }
    }

    void IncreaseDifficult()
    {
        switch(difficult)
        {
            case 1:
                fallTime = 0.4f;
                break;
            case 2:
                fallTime = 0.2f;
                break;
        }
    }
}