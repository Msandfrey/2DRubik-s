using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotationButtons : MonoBehaviour
{
    GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        foreach (Transform child in transform)
        {
            switch (child.gameObject.name[0])
            {
                case 'S': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { Scramble(); }); break;
                case 'L': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { LSwipe(child.gameObject.name.Length == 1); }); break;
                case 'R': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { RSwipe(child.gameObject.name.Length == 1); }); break;
                case 'U': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { USwipe(child.gameObject.name.Length == 1); }); break;
                case 'D': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { DSwipe(child.gameObject.name.Length == 1); }); break;
                case 'F': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { FSwipe(child.gameObject.name.Length == 1); }); break;
                case 'M': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { MSwipe(child.gameObject.name.Length == 1); }); break;
                case 'E': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { ESwipe(child.gameObject.name.Length == 1); }); break;
                case 'x': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { XRotate(child.gameObject.name.Length == 1); }); break;
                case 'y': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { YRotate(child.gameObject.name.Length == 1); }); break;
                case 'z': child.gameObject.GetComponent<Button>().onClick.AddListener(delegate { ZRotate(child.gameObject.name.Length == 1); }); break;
                default:  break;
            }

        }

    }

    public void Scramble()
    {
        RSwipe(true);
        USwipe(true);
        DSwipe(false);
        RSwipe(true);
        LSwipe(true);
        FSwipe(false);
        MSwipe(true);
        MSwipe(true);
        ESwipe(true);
        FSwipe(true);
        RSwipe(true);
        USwipe(false);
    }

    public void RSwipe(bool isNormal)//rotate the right face clockwise (col2 up)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(-1, false, 2, true);
        }
        else
        {
            gridManager.UpdateGrid(1, false, 2, true);
        }
    }
    public void LSwipe(bool isNormal)//rotate the left face clockwise (col0 down)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(1, false, 0, true);
        }
        else
        {
            gridManager.UpdateGrid(-1, false, 0, true);
        }
    }
    public void DSwipe(bool isNormal)//rotate the down face clockwise (row2 right)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(1, true, 2, false);
        }
        else
        {
            gridManager.UpdateGrid(-1, true, 2, false);
        }
    }
    public void USwipe(bool isNormal)//rotate the up face clockwise (row0 left)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(-1, true, 0, false);
        }
        else
        {
            gridManager.UpdateGrid(1, true, 0, false);
        }
    }
    public void FSwipe(bool isNormal)//rotate the front face clockwise 
    {
        if (isNormal)
        {
            gridManager.RotateFaceClockwise(true, 0, true);
        }
        else
        {
            gridManager.RotateFaceCounterClockwise(true, 0, true);
        }
        gridManager.UpdateMainNine(false);
    }
    public void MSwipe(bool isNormal)//rotate the middle vertical column down (col1)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(1, false, 1, true);
        }
        else
        {
            gridManager.UpdateGrid(-1, false, 1, true);
        }
    }
    public void ESwipe(bool isNormal)//rotate the middle horizontal row right (row1)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(1, true, 1, false);
        }
        else
        {
            gridManager.UpdateGrid(-1, true, 1, false);
        }
    }
    public void XRotate(bool isNormal)//rotate whole cube and face up (dir R)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(-1, false, 0, true);
            gridManager.UpdateGrid(-1, false, 1, true);
            gridManager.UpdateGrid(-1, false, 2, true);
        }
        else
        {
            gridManager.UpdateGrid(1, false, 0, true);
            gridManager.UpdateGrid(1, false, 1, true);
            gridManager.UpdateGrid(1, false, 2, true);
        }
    }
    public void YRotate(bool isNormal)//rotate whole cube and face left (dir U)
    {
        if (isNormal)
        {
            gridManager.UpdateGrid(-1, true, 0, false);
            gridManager.UpdateGrid(-1, true, 1, false);
            gridManager.UpdateGrid(-1, true, 2, false);
        }
        else
        {
            gridManager.UpdateGrid(1, true, 0, false);
            gridManager.UpdateGrid(1, true, 1, false);
            gridManager.UpdateGrid(1, true, 2, false);
        }
    }
    public void ZRotate(bool isNormal)//rotate whole cube clockwise(dir F)
    {
        //up
        gridManager.UpdateGrid(-1, false, 0, true);
        gridManager.UpdateGrid(-1, false, 1, true);
        gridManager.UpdateGrid(-1, false, 2, true);

        if (isNormal)
        {
            //left
            gridManager.UpdateGrid(-1, true, 0, false);
            gridManager.UpdateGrid(-1, true, 1, false);
            gridManager.UpdateGrid(-1, true, 2, false);
        }
        else
        {
            //right
            gridManager.UpdateGrid(1, true, 0, false);
            gridManager.UpdateGrid(1, true, 1, false);
            gridManager.UpdateGrid(1, true, 2, false);
        }

        //down
        gridManager.UpdateGrid(1, false, 0, true);
        gridManager.UpdateGrid(1, false, 1, true);
        gridManager.UpdateGrid(1, false, 2, true);
    }
}
