using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour
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
    bool dragging = false;
    public float sensitivity = 0.01f;//adjust this for sliding speed
    bool leftRight = false;
    bool upDown = false;

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
            dragging = true;
            ped = new PointerEventData(ev);
            ped.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach (RaycastResult result in results)//save the name of the first object (should be a number between 0-8)
            {
                SetCurrentRowAndCol(result.gameObject.name);
                break;
            }
        }
        if (dragging)
        {
            if (leftRight)//if dragging a row left or right
            {
                if (gridManager.currentRow == gridManager.row0)
                {
                    DragLeftRight(gridManager.gridPanelsRow0);
                }
                else if (gridManager.currentRow == gridManager.row1)
                {
                    DragLeftRight(gridManager.gridPanelsRow1);
                }
                else if (gridManager.currentRow == gridManager.row2)
                {
                    DragLeftRight(gridManager.gridPanelsRow2);
                }
            }
            else if (upDown)//if dragging a col up or down
            {
                if (gridManager.currentCol == gridManager.col0)
                {
                    DragUpDown(gridManager.gridPanelsCol0);
                }
                else if (gridManager.currentCol == gridManager.col1)
                {
                    DragUpDown(gridManager.gridPanelsCol1);
                }
                else if (gridManager.currentCol == gridManager.col2)
                {
                    DragUpDown(gridManager.gridPanelsCol2);
                }
            }
            else if ((-1 * Input.mousePosition.x + firstPressPos.x) > 0.5f || (-1 * Input.mousePosition.x + firstPressPos.x) < -0.5f)//-0.5f > xmove||ment > 0.5(if lots of x movement)
            {
                leftRight = true;
            }
            else if ((-1 * Input.mousePosition.y + firstPressPos.y) > 0.5f || (-1 * Input.mousePosition.y + firstPressPos.y) < -0.5f)//-0.5f > ymove||ment > 0.5)(if lots of y movement)
            {
                upDown = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (leftRight)
                {
                    if (gridManager.currentRow == gridManager.row0)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow0, gridManager.originalPositionsRow0);
                        //MoveToCorrectPos(gridManager.gridPanelsRow0);
                        ResetPanels(gridManager.gridPanelsRow0, gridManager.originalPositionsRow0);
                        gridManager.UpdateGrid(i[0], true, 0, false);//(-1,0,1), true, 0, false
                    }
                    else if (gridManager.currentRow == gridManager.row1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow1, gridManager.originalPositionsRow1);
                        //MoveToCorrectPos(gridManager.gridPanelsRow1);
                        ResetPanels(gridManager.gridPanelsRow1, gridManager.originalPositionsRow1);
                        gridManager.UpdateGrid(i[0], true, 1, false);//(-1,0,1), true, 1, false
                    }
                    else if (gridManager.currentRow == gridManager.row2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsRow2, gridManager.originalPositionsRow2);
                        //MoveToCorrectPos(gridManager.gridPanelsRow2);
                        ResetPanels(gridManager.gridPanelsRow2, gridManager.originalPositionsRow2);
                        gridManager.UpdateGrid(i[0], true, 2, false);//(-1,0,1), true, 2, false
                    }
                }
                if (upDown)
                {
                    if (gridManager.currentCol == gridManager.col0)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol0, gridManager.originalPositionsCol0);
                        //MoveToCorrectPos(gridManager.gridPanelsCol0);
                        ResetPanels(gridManager.gridPanelsCol0, gridManager.originalPositionsCol0);
                        gridManager.UpdateGrid(i[1], false, 0, true);//(-1,0,1), false, 0, true
                    }
                    else if (gridManager.currentCol == gridManager.col1)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol1, gridManager.originalPositionsCol1);
                        //MoveToCorrectPos(gridManager.gridPanelsCol1);
                        ResetPanels(gridManager.gridPanelsCol1, gridManager.originalPositionsCol1);
                        gridManager.UpdateGrid(i[1], false, 1, true);//(-1,0,1), false, 1, true
                    }
                    else if (gridManager.currentCol == gridManager.col2)
                    {
                        int[] i = GetDirection(gridManager.gridPanelsCol2, gridManager.originalPositionsCol2);
                        //MoveToCorrectPos(gridManager.gridPanelsCol2);
                        ResetPanels(gridManager.gridPanelsCol2, gridManager.originalPositionsCol2);
                        gridManager.UpdateGrid(i[1], false, 2, true);//(-1,0,1), false, 2, true
                    }
                }
                dragging = false;
                leftRight = false;
                upDown = false;
            }
        }
    }

    private void DragLeftRight(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + firstPressPos);
        rotation.x = ((mouseOffset.x) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach (GameObject tile in rowCol)
        {
            if (tile.transform.position.x + rotation.x > gridManager.originalPositionsRow0[i].x + 120)//do not go past one tile to the right
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.x + rotation.x < gridManager.originalPositionsRow0[i].x - 120)//do not go past one tile to the left
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
            }
            i++;
        }

        firstPressPos = Input.mousePosition;
    }
    private void DragUpDown(List<GameObject> rowCol)
    {
        Vector3 rotation = Vector3.zero;

        Vector3 mouseOffset = (-1 * Input.mousePosition + firstPressPos);
        rotation.y = ((mouseOffset.y) * sensitivity * -1f);//works but get it to work better
        int i = 0;
        foreach (GameObject tile in rowCol)
        {
            if (tile.transform.position.y + rotation.y > gridManager.originalPositionsCol0[i].y + 120)//do not go past one tile up
            {
                tile.transform.Translate(Vector3.zero, Space.Self);
            }
            else if (tile.transform.position.y + rotation.y < gridManager.originalPositionsCol0[i].y - 120)//do not go past one tile down
            {

            }
            else
            {
                tile.transform.Translate(rotation, Space.Self);
            }
            //tile.transform.Translate(rotation, Space.Self);
            i++;
        }

        firstPressPos = Input.mousePosition;
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

    bool RightSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y > -10 && swipe.y < 10;
    }
    bool LeftSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y > -10 && swipe.y < 10;
    }
    bool DownSwipe(Vector2 swipe)
    {
        return swipe.y > 0 && swipe.x > -10 && swipe.x < 10;
    }
    bool UpSwipe(Vector2 swipe)
    {
        return swipe.y < 0 && swipe.x > -10 && swipe.x < 10;
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
        if (panels[2].transform.position.x < originalPos[2].x - 45f)
        {
            results[0] = -1;//left
        }
        else if (panels[2].transform.position.x > originalPos[2].x + 45f)
        {
            results[0] = 1;//right
        }
        if (panels[2].transform.position.y < originalPos[2].y - 45f)
        {
            results[1] = 1;//down
        }
        else if (panels[2].transform.position.y > originalPos[2].y + 45f)
        {
            results[1] = -1;//up
        }
        return results;
    }

    public void ResetPanels(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        int i = 0;
        foreach (GameObject pan in rowCol)
        {
            pan.transform.position = originalRowCol[i];
            i++;
        }
    }
}
