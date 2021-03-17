using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridCursor : MonoBehaviour
{
    //raycaster
    GraphicRaycaster gr;
    PointerEventData ped;
    EventSystem ev;
    
    //unused but maybe should use?
    Color[] currentRow;
    Color[] currentCol;

    //for swiping
    Vector3 firstPressPos;

    //the grid
    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        //for raycasting the click
        gr = FindObjectOfType<GraphicRaycaster>();
        ev = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            firstPressPos = Input.mousePosition;//get the location of the initial click
            ped = new PointerEventData(ev);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach(RaycastResult result in results)//save the name of the first object (should be a number between 0-8)
            {
                SetCurrentRowAndCol(result.gameObject.name);
                break;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube clockwise
        {
            //up
            gridManager.UpdateGrid(-1, false, 0, true);
            gridManager.UpdateGrid(-1, false, 1, true);
            gridManager.UpdateGrid(-1, false, 2, true);

            //left
            gridManager.UpdateGrid(-1, true, 0, false);
            gridManager.UpdateGrid(-1, true, 1, false);
            gridManager.UpdateGrid(-1, true, 2, false);

            //down
            gridManager.UpdateGrid(1, false, 0, true);
            gridManager.UpdateGrid(1, false, 1, true);
            gridManager.UpdateGrid(1, false, 2, true);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube counter-clockwise
        {
            //up
            gridManager.UpdateGrid(-1, false, 0, true);
            gridManager.UpdateGrid(-1, false, 1, true);
            gridManager.UpdateGrid(-1, false, 2, true);

            //right
            gridManager.UpdateGrid(1, true, 0, false);
            gridManager.UpdateGrid(1, true, 1, false);
            gridManager.UpdateGrid(1, true, 2, false);

            //down
            gridManager.UpdateGrid(1, false, 0, true);
            gridManager.UpdateGrid(1, false, 1, true);
            gridManager.UpdateGrid(1, false, 2, true);
        }
        else if(Input.GetKeyDown(KeyCode.E))//rotate main face clockwise
        {
            gridManager.RotateFaceClockwise(true, 0, true);
            gridManager.UpdateMainNine(false);
        }
        else if (Input.GetKeyDown(KeyCode.Q))//rotate main face counter-clockwise
        {
            gridManager.RotateFaceCounterClockwise(true, 0, true);
            gridManager.UpdateMainNine(false);
        }
        else if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube right
        {
            gridManager.UpdateGrid(1, true, 0, false);         
            gridManager.UpdateGrid(1, true, 1, false);         
            gridManager.UpdateGrid(1, true, 2, false);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube left
        {
            gridManager.UpdateGrid(-1, true, 0, false);
            gridManager.UpdateGrid(-1, true, 1, false);
            gridManager.UpdateGrid(-1, true, 2, false);
        }
        else if (Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube up
        {
            gridManager.UpdateGrid(-1, false, 0, true);            
            gridManager.UpdateGrid(-1, false, 1, true);            
            gridManager.UpdateGrid(-1, false, 2, true);            
        }
        else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))//rotate whole cube down
        {
            gridManager.UpdateGrid(1, false, 0, true);            
            gridManager.UpdateGrid(1, false, 1, true);            
            gridManager.UpdateGrid(1, false, 2, true);            
        }
        else if (Input.GetKeyDown(KeyCode.A))//rotate row left
        {
            if (gridManager.currentRow == gridManager.row0)
            {
                gridManager.UpdateGrid(-1, true, 0, false);
            }
            else if (gridManager.currentRow == gridManager.row1)
            {
                gridManager.UpdateGrid(-1, true, 1, false);
            }
            else if (gridManager.currentRow == gridManager.row2)
            {
                gridManager.UpdateGrid(-1, true, 2, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))//rotate row right
        {
            if (gridManager.currentRow == gridManager.row0)
            {
                gridManager.UpdateGrid(1, true, 0, false);
            }
            else if (gridManager.currentRow == gridManager.row1)
            {
                gridManager.UpdateGrid(1, true, 1, false);
            }
            else if (gridManager.currentRow == gridManager.row2)
            {
                gridManager.UpdateGrid(1, true, 2, false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))//rotate col up
        {
            if (gridManager.currentCol == gridManager.col0)
            {
                gridManager.UpdateGrid(-1, false, 0, true);
            }
            else if (gridManager.currentCol == gridManager.col1)
            {
                gridManager.UpdateGrid(-1, false, 1, true);
            }
            else if (gridManager.currentCol == gridManager.col2)
            {
                gridManager.UpdateGrid(-1, false, 2, true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))//rotate col down
        {
            if (gridManager.currentCol == gridManager.col0)
            {
                gridManager.UpdateGrid(1, false, 0, true);
            }
            else if (gridManager.currentCol == gridManager.col1)
            {
                gridManager.UpdateGrid(1, false, 1, true);
            }
            else if (gridManager.currentCol == gridManager.col2)
            {
                gridManager.UpdateGrid(1, false, 2, true);
            }
        }
    }
    public void MoveToCorrectPos(List<GameObject> rowCol)//snap the grid in place
    {
        int[] i = new int[2];
        foreach (GameObject tile in rowCol)
        {
            float scale = 60f;
            Vector3 vec = tile.transform.localPosition;
            vec.x = Mathf.Round(vec.x / scale) * scale;
            vec.y = Mathf.Round(vec.y / scale) * scale;
            vec.z = Mathf.Round(vec.z / scale) * scale;
            tile.transform.localPosition = vec;
        }
    }

    void SetCurrentRowAndCol(string tile)//based on click sets currentRow/Col in the grid
    {
        switch (tile)
        {
            case "0":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col0;
                break;
            case "1":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col1;
                break;
            case "2":
                gridManager.currentRow = gridManager.row0;
                gridManager.currentCol = gridManager.col2;
                break;
            case "3":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col0;
                break;
            case "4":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col1;
                break;
            case "5":
                gridManager.currentRow = gridManager.row1;
                gridManager.currentCol = gridManager.col2;
                break;
            case "6":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col0;
                break;
            case "7":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col1;
                break;
            case "8":
                gridManager.currentRow = gridManager.row2;
                gridManager.currentCol = gridManager.col2;
                break;
        }
    }

    //based on the position of the center panel when click released, decide if movement is left, right, up, down or none
    int[] GetDirection(List<GameObject> panels, List<Vector3> originalPos)
    {
        int[] results = { 0, 0 };//x, y
        if(panels[2].transform.position.x < originalPos[2].x - 1.2f)
        {
            results[0] = -1;//left
        }
        else if(panels[2].transform.position.x > originalPos[2].x + 1.2f)
        {
            results[0] = 1;//right
        }
        if (panels[2].transform.position.y < originalPos[2].y - 1.2f)
        {
            results[1] = 1;//down
        }
        else if (panels[2].transform.position.y > originalPos[2].y + 1.2f)
        {
            results[1] = -1;//up
        }
        return results;
    }
}
