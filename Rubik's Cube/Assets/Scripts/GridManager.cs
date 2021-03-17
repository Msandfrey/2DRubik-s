using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //unused consts
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private const float GRID_HEIGHT = 3f;
    private const float GRID_WIDTH = 3f;

    private GlobalControl globalControl;
    

    //keep track of the current row and col so we know which ones to slide
    [HideInInspector] public Color[] currentRow;
    [HideInInspector] public Color[] currentCol;

    [HideInInspector] public Color[] colorSelection;
    
    //The rows and cols of colors - to be rooms later
    public Color[] row0 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] row1 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] row2 = { Color.white, Color.white, Color.white, Color.red, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.magenta, Color.magenta, Color.magenta };
    public Color[] col0 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col1 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    public Color[] col2 = { Color.white, Color.white, Color.white, Color.green, Color.green, Color.green, Color.yellow, Color.yellow, Color.yellow, Color.blue, Color.blue, Color.blue };
    //The grid panels by row and col ( index 1 - 3 is the main visible grid; 0 and 5 are previews to other rooms necessary for clean sliding)
    public List<GameObject> gridPanelsRow0;
    public List<GameObject> gridPanelsRow1;
    public List<GameObject> gridPanelsRow2;
    public List<GameObject> gridPanelsCol0;
    public List<GameObject> gridPanelsCol1;
    public List<GameObject> gridPanelsCol2;
    //Where to store the original positions of the rows and cols of panels
    [HideInInspector] public List<Vector3> originalPositionsRow0 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsRow1 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsRow2 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol0 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol1 = new List<Vector3>();
    [HideInInspector] public List<Vector3> originalPositionsCol2 = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        globalControl = FindObjectOfType<GlobalControl>();
        if (globalControl != null)
        {
            colorSelection = globalControl.GetColors();
            if(colorSelection != null)
            {
                SetRowColors(colorSelection);
                SetColColors(colorSelection);
            }
        }
        //save the original positions of all the grid panels so we can reset them when necessary
        SetOriginalPos(gridPanelsRow0, originalPositionsRow0);
        SetOriginalPos(gridPanelsRow1, originalPositionsRow1);
        SetOriginalPos(gridPanelsRow2, originalPositionsRow2);
        SetOriginalPos(gridPanelsCol0, originalPositionsCol0);
        SetOriginalPos(gridPanelsCol1, originalPositionsCol1);
        SetOriginalPos(gridPanelsCol2, originalPositionsCol2);
    }

    // Update is called once per frame
    void Update()
    {
        DrawColors();
    }

    public void SetRowColors(Color[] colorPalette)
    {
        for(int i = 0; i<row0.Length; i++)
        {
            if(i < 3)
            {
                row0[i] = colorPalette[5];
                row1[i] = colorPalette[5];
                row2[i] = colorPalette[5];
            }
            else if(i < 6)
            {
                row0[i] = colorPalette[0];
                row1[i] = colorPalette[0];
                row2[i] = colorPalette[0];
            }
            else if (i < 9)
            {
                row0[i] = colorPalette[2];
                row1[i] = colorPalette[2];
                row2[i] = colorPalette[2];
            }
            else
            {
                row0[i] = colorPalette[3];
                row1[i] = colorPalette[3];
                row2[i] = colorPalette[3];
            }

        }
    }
    public void SetColColors(Color[] colorPalette)
    {
        for (int i = 0; i < col0.Length; i++)
        {
            if (i < 3)
            {
                col0[i] = colorPalette[5];
                col1[i] = colorPalette[5];
                col2[i] = colorPalette[5];
            }
            else if (i < 6)
            {
                col0[i] = colorPalette[4];
                col1[i] = colorPalette[4];
                col2[i] = colorPalette[4];
            }
            else if (i < 9)
            {
                col0[i] = colorPalette[2];
                col1[i] = colorPalette[2];
                col2[i] = colorPalette[2];
            }
            else
            {
                col0[i] = colorPalette[1];
                col1[i] = colorPalette[1];
                col2[i] = colorPalette[1];
            }

        }
    }

    //shifts up for cols or left for rows
    //just adjusts the data in one row
    //needs to be adjusted in overlapping rows after
    public Color[] ShiftUpLeft(Color[] shifted)
    {
        Color[] temp = new Color[shifted.Length];
        for (int i = 0; i < shifted.Length; i++)
        {
            if (i == shifted.Length - 1)
            {
                temp[i] = shifted[0];
            }
            else
            {
                temp[i] = shifted[i + 1];
            }
        }
        return temp;
    }

    //shifts down for cols or right for rows
    //just adjusts the data in one row
    //needs to be adjusted in overlapping rows after
    public Color[] ShiftDownRight(Color[] shifted)
    {
        Color[] temp = new Color[shifted.Length];
        for (int i = 0; i < shifted.Length; i++)
        {
            if (i == 0)
            {
                temp[i] = shifted[shifted.Length - 1];
            }
            else
            {
                temp[i] = shifted[i - 1];
            }
        }
        return temp;
    }

    public void RotateFaceClockwise(bool upDown, int faceNum, bool isFront)
    {
        Color[] temp0 = new Color[row0.Length];
        Color[] temp1 = new Color[row0.Length];
        Color[] temp2 = new Color[row0.Length];
        int faceID = faceNum * 3;
        //if rotated left or right update cols
        if (!upDown)
        {
            for (int i = 0; i < row0.Length; i++)
            {
                if (i == faceID)
                {
                    temp0[faceID] = col0[faceID + 2];
                    temp1[faceID] = col0[faceID + 1];
                    temp2[faceID] = col0[faceID];
                }
                else if (i == faceID + 1)
                {
                    temp0[faceID + 1] = col1[faceID + 2];
                    temp1[faceID + 1] = col1[faceID + 1];
                    temp2[faceID + 1] = col1[faceID];
                }
                else if (i == faceID + 2)
                {
                    temp0[faceID + 2] = col2[faceID + 2];
                    temp1[faceID + 2] = col2[faceID + 1];
                    temp2[faceID + 2] = col2[faceID];
                }
                else
                {
                    temp0[i] = col0[i];
                    temp1[i] = col1[i];
                    temp2[i] = col2[i];
                }
            }
            if (isFront)
            {
                temp0[11] = row2[11];
                temp1[11] = row1[11];
                temp2[11] = row0[11];
                temp0[3] = row2[3];
                temp1[3] = row1[3];
                temp2[3] = row0[3];
                row0[11] = col2[3];
                row1[11] = col1[3];
                row2[11] = col0[3];
                row0[3] = col0[11];
                row1[3] = col1[11];
                row2[3] = col2[11];
            }
            col0 = temp0;
            col1 = temp1;
            col2 = temp2;
        }        
        //if rotated up or down update rows
        else
        {
            for (int i = 0; i < row0.Length; i++)
            {
                if (i == faceID)
                {
                    temp0[faceID] = row2[faceID];
                    temp1[faceID] = row2[faceID + 1];
                    temp2[faceID] = row2[faceID+2];
                }
                else if (i == faceID + 1)
                {
                    temp0[faceID + 1] = row1[faceID];
                    temp1[faceID + 1] = row1[faceID + 1];
                    temp2[faceID + 1] = row1[faceID+2];
                }
                else if (i == faceID + 2)
                {
                    temp0[faceID + 2] = row0[faceID];
                    temp1[faceID + 2] = row0[faceID + 1];
                    temp2[faceID + 2] = row0[faceID+2];
                }
                else
                {
                    temp0[i] = row0[i];
                    temp1[i] = row1[i];
                    temp2[i] = row2[i];
                }
            }
            if (isFront)
            {
                temp0[11] = col0[3];
                temp1[11] = col1[3];
                temp2[11] = col2[3];
                temp0[3] = col0[11];
                temp1[3] = col1[11];
                temp2[3] = col2[11];
                col0[11] = row2[11];
                col1[11] = row1[11];
                col2[11] = row0[11];
                col0[3] = row2[3];
                col1[3] = row1[3];
                col2[3] = row0[3];
            }
            row0 = temp0;
            row1 = temp1;
            row2 = temp2;
        }
    }
    public void RotateFaceCounterClockwise(bool upDown, int faceNum, bool isFront)
    {
        Color[] temp0 = new Color[row0.Length];
        Color[] temp1 = new Color[row0.Length];
        Color[] temp2 = new Color[row0.Length];
        int faceID = faceNum * 3;
        //if rotated left or right update cols
        if (!upDown)
        {
            for (int i = 0; i < row0.Length; i++)
            {
                if (i == faceID)
                {
                    temp0[faceID] = col2[faceID];
                    temp1[faceID] = col2[faceID + 1];
                    temp2[faceID] = col2[faceID + 2];
                }
                else if (i == faceID + 1)
                {
                    temp0[faceID + 1] = col1[faceID];
                    temp1[faceID + 1] = col1[faceID + 1];
                    temp2[faceID + 1] = col1[faceID + 2];
                }
                else if (i == faceID + 2)
                {
                    temp0[faceID + 2] = col0[faceID];
                    temp1[faceID + 2] = col0[faceID + 1];
                    temp2[faceID + 2] = col0[faceID + 2];
                }
                else
                {
                    temp0[i] = col0[i];
                    temp1[i] = col1[i];
                    temp2[i] = col2[i];
                }
            }
            if (isFront)
            {
                temp0[11] = row0[3];
                temp1[11] = row1[3];
                temp2[11] = row2[3];
                temp0[3] = row0[11];
                temp1[3] = row1[11];
                temp2[3] = row2[11];
                row0[3] = col2[3];
                row1[3] = col1[3];
                row2[3] = col0[3];
                row0[11] = col2[11];
                row1[11] = col1[11];
                row2[11] = col0[11];
            }
            col0 = temp0;
            col1 = temp1;
            col2 = temp2;
        }        
        //if rotated up or down update rows
        else
        {
            for (int i = 0; i < row0.Length; i++)
            {
                if (i == faceID)
                {
                    temp0[faceID] = row0[faceID + 2];
                    temp1[faceID] = row0[faceID + 1];
                    temp2[faceID] = row0[faceID];
                }
                else if (i == faceID + 1)
                {
                    temp0[faceID + 1] = row1[faceID + 2];
                    temp1[faceID + 1] = row1[faceID + 1];
                    temp2[faceID + 1] = row1[faceID];
                }
                else if (i == faceID + 2)
                {
                    temp0[faceID + 2] = row2[faceID + 2];
                    temp1[faceID + 2] = row2[faceID + 1];
                    temp2[faceID + 2] = row2[faceID];
                }
                else
                {
                    temp0[i] = row0[i];
                    temp1[i] = row1[i];
                    temp2[i] = row2[i];
                }
            }
            if (isFront)
            {
                temp0[3] = col2[3];
                temp1[3] = col1[3];
                temp2[3] = col0[3];
                temp0[11] = col2[11];
                temp1[11] = col1[11];
                temp2[11] = col0[11];
                col0[11] = row0[3];
                col1[11] = row1[3];
                col2[11] = row2[3];
                col0[3] = row0[11];
                col1[3] = row1[11];
                col2[3] = row2[11];
            }
            row0 = temp0;
            row1 = temp1;
            row2 = temp2;
        }
    }

    //makes sure that the elements in the first 3 spots of each array match
    public void UpdateMainNine(bool movedVertical)
    {
        if (movedVertical)//if you moved a col set the rows
        {
            row0[0] = col0[0];
            row0[1] = col1[0];  //            |             |
            row0[2] = col2[0];  //r1[0],c1[0] | r1[1],c2[0] | r1[2],c3[0]
            row1[0] = col0[1];  //_______________________________________
            row1[1] = col1[1];  //
            row1[2] = col2[1];  //r2[0],c1[1] | r2[1],c2[1] | r2[2],c3[1]  
            row2[0] = col0[2];  //_______________________________________
            row2[1] = col1[2];  //r3[0],c1[2] | r3[1],c2[2] | r3[2],c3[2]
            row2[2] = col2[2];  //            |             |
        }
        else// if you moved a row set the cols
        {
            col0[0] = row0[0];
            col1[0] = row0[1];
            col2[0] = row0[2];
            col0[1] = row1[0];
            col1[1] = row1[1];
            col2[1] = row1[2];
            col0[2] = row2[0];
            col1[2] = row2[1];
            col2[2] = row2[2];
        }
    }

    public void ThirdGroupMatching(bool movedVertical)// this insures that the 3rd grouping of 9 in the rows and columns are the same ([6,7,8] of each)
    {
        if (movedVertical)
        {
            row0[6] = col2[8];
            row0[7] = col1[8];
            row0[8] = col0[8];
            row1[6] = col2[7];
            row1[7] = col1[7];
            row1[8] = col0[7];
            row2[6] = col2[6];
            row2[7] = col1[6];
            row2[8] = col0[6];
        }
        else
        {
            col0[6] = row2[8];
            col1[6] = row2[7];
            col2[6] = row2[6];
            col0[7] = row1[8];
            col1[7] = row1[7];
            col2[7] = row1[6];
            col0[8] = row0[8];
            col1[8] = row0[7];
            col2[8] = row0[6];
        }
    }

    public void DrawColors()
    {
        DrawRow(gridPanelsRow0, row0);
        DrawRow(gridPanelsRow1, row1);
        DrawRow(gridPanelsRow2, row2);
        DrawRow(gridPanelsCol0, col0);
        DrawRow(gridPanelsCol1, col1);
        DrawRow(gridPanelsCol2, col2);
    }

    //color each of the tiles in each row
    //index 0 will be the last value in the Color/Room array
    //index 1-4 should be the same as its counterpart in the Color/Room array -1
    public void DrawRow(List<GameObject> panelRow, Color[] rowSource)
    {
        int i = 0;
        foreach (GameObject tile in panelRow)
        {
            if (i == 0)
            {
                tile.GetComponent<Image>().color = rowSource[rowSource.Length - 1];
            }
            else
            {
                tile.GetComponent<Image>().color = rowSource[i - 1];
            }
            i++;
        }
    }

    //move the grid panels back to their original position
    public void ResetPanels(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        int i = 0;
        foreach (GameObject pan in rowCol)
        {
            pan.transform.position = originalRowCol[i];
            i++;
        }
    }

    //set the original pos
    void SetOriginalPos(List<GameObject> rowCol, List<Vector3> originalRowCol)
    {
        foreach (GameObject pan in rowCol)
        {
            originalRowCol.Add(new Vector3(pan.transform.position.x, pan.transform.position.y));
        }
    }

    //main update of the grid
    //shift rows, update arrays
    public void UpdateGrid(int upLeft, bool isRow, int rowColNumber, bool updown)
    {
        if (isRow)
        {
            if(upLeft == -1)//moving left
            {
                switch (rowColNumber)
                {
                    case 0:
                        row0 = ShiftUpLeft(row0);
                        currentRow = row0;
                        row0 = ShiftUpLeft(row0);
                        currentRow = row0;
                        row0 = ShiftUpLeft(row0);
                        currentRow = row0;
                        //rotate the above face clockwise
                        RotateFaceClockwise(false, 3, false);
                        //ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        row1 = ShiftUpLeft(row1);
                        currentRow = row1;
                        row1 = ShiftUpLeft(row1);
                        currentRow = row1;
                        row1 = ShiftUpLeft(row1);
                        currentRow = row1;
                        //ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        row2 = ShiftUpLeft(row2);
                        currentRow = row2;
                        row2 = ShiftUpLeft(row2);
                        currentRow = row2;
                        row2 = ShiftUpLeft(row2);
                        currentRow = row2;
                        //rotate the below face counter-clockwise
                        RotateFaceCounterClockwise(false, 1, false);
                        //ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
            else if(upLeft == 1)//moving right
            {
                switch (rowColNumber)
                {
                    case 0:
                        row0 = ShiftDownRight(row0);
                        currentRow = row0;
                        row0 = ShiftDownRight(row0);
                        currentRow = row0;
                        row0 = ShiftDownRight(row0);
                        currentRow = row0;
                        //rotate the above face counterclockwise
                        RotateFaceCounterClockwise(false, 3, false);
                        //ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        row1 = ShiftDownRight(row1);
                        currentRow = row1;
                        row1 = ShiftDownRight(row1);
                        currentRow = row1;
                        row1 = ShiftDownRight(row1);
                        currentRow = row1;
                        //ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        row2 = ShiftDownRight(row2);
                        currentRow = row2;
                        row2 = ShiftDownRight(row2);
                        currentRow = row2;
                        row2 = ShiftDownRight(row2);
                        currentRow = row2;
                        //rotate the below face clockwise
                        RotateFaceClockwise(false, 1, false);
                        //ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
            else if(upLeft == 0)
            {
                switch (rowColNumber)
                {
                    case 0:
                        //ResetPanels(gridPanelsRow0, originalPositionsRow0);
                        break;
                    case 1:
                        //ResetPanels(gridPanelsRow1, originalPositionsRow1);
                        break;
                    case 2:
                        //ResetPanels(gridPanelsRow2, originalPositionsRow2);
                        break;
                }
            }
        }
        else
        {
            if (upLeft == -1)//moving up
            {
                switch (rowColNumber)
                {
                    case 0:
                        col0 = ShiftUpLeft(col0);
                        currentCol = col0;
                        col0 = ShiftUpLeft(col0);
                        currentCol = col0;
                        col0 = ShiftUpLeft(col0);
                        currentCol = col0;
                        //rotate the left face counter-clockwise
                        RotateFaceCounterClockwise(true, 3, false);
                        //ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        col1 = ShiftUpLeft(col1);
                        currentCol = col1;
                        col1 = ShiftUpLeft(col1);
                        currentCol = col1;
                        col1 = ShiftUpLeft(col1);
                        currentCol = col1;
                        //ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        col2 = ShiftUpLeft(col2);
                        currentCol = col2;
                        col2 = ShiftUpLeft(col2);
                        currentCol = col2;
                        col2 = ShiftUpLeft(col2);
                        currentCol = col2;
                        //rotate the right face clockwise
                        RotateFaceClockwise(true, 1, false);
                        //ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
            else if (upLeft == 1)//moving down
            {
                switch (rowColNumber)
                {
                    case 0:
                        col0 = ShiftDownRight(col0);
                        currentCol = col0;
                        col0 = ShiftDownRight(col0);
                        currentCol = col0;
                        col0 = ShiftDownRight(col0);
                        currentCol = col0;
                        //rotate the left face clockwise
                        RotateFaceClockwise(true, 3, false);
                        //ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        col1 = ShiftDownRight(col1);
                        currentCol = col1;
                        col1 = ShiftDownRight(col1);
                        currentCol = col1;
                        col1 = ShiftDownRight(col1);
                        currentCol = col1;
                        //ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        col2 = ShiftDownRight(col2);
                        currentCol = col2;
                        col2 = ShiftDownRight(col2);
                        currentCol = col2;
                        col2 = ShiftDownRight(col2);
                        currentCol = col2;
                        //rotate the right face counter-clockwise
                        RotateFaceCounterClockwise(true, 1, false);
                        //ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
            else if (upLeft == 0)
            {
                switch (rowColNumber)
                {
                    case 0:
                        //ResetPanels(gridPanelsCol0, originalPositionsCol0);
                        break;
                    case 1:
                        //ResetPanels(gridPanelsCol1, originalPositionsCol1);
                        break;
                    case 2:
                        //ResetPanels(gridPanelsCol2, originalPositionsCol2);
                        break;
                }
            }
        }
        UpdateMainNine(updown);
        ThirdGroupMatching(updown);
    }
}
