using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMap : MonoBehaviour
{
    private GridManager gridManager;

    public Transform mapRow0;
    public Transform mapRow1;
    public Transform mapRow2;
    public Transform mapCol0;
    public Transform mapCol1;
    public Transform mapCol2;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMap(gridManager.row0, mapRow0);
        UpdateMap(gridManager.row1, mapRow1);
        UpdateMap(gridManager.row2, mapRow2);
        UpdateMap(gridManager.col0, mapCol0, false);
        UpdateMap(gridManager.col1, mapCol1, false);
        UpdateMap(gridManager.col2, mapCol2, false);
    }

    void UpdateMap(Color[] rowCol, Transform mapGroups, bool isRow = true)
    {
        int i;
        if (isRow)
        {
            i = 0;
        }
        else
        {
            i = 3;
        }
        foreach (Transform cell in mapGroups)
        {
            if (rowCol[i] == Color.magenta)
            {
                cell.GetComponent<Image>().color = Color.magenta;//orange, but actually magenta
            }
            else if (rowCol[i] == Color.red)
            {
                cell.GetComponent<Image>().color = Color.red;
            }
            else if (rowCol[i] == Color.white)
            {
                cell.GetComponent<Image>().color = Color.white;
            }
            else if (rowCol[i] == Color.green)
            {
                cell.GetComponent<Image>().color = Color.green;
            }
            else if (rowCol[i] == Color.yellow)
            {
                cell.GetComponent<Image>().color = Color.yellow;
            }
            else if (rowCol[i] == Color.blue)
            {
                cell.GetComponent<Image>().color = Color.blue;
            }
            else
            {
                cell.GetComponent<Image>().color = Color.gray;
            }
            i++;
        }
    }
}
