using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedCellContainer : MonoBehaviour
{
    List<planeCoord> cellsChangedThisClick = new List<planeCoord>();

    public void add (planeCoord coordinateName) {
        cellsChangedThisClick.Add(coordinateName);
    }

    public void print () {
        string toPrint = null;
        foreach (planeCoord entry in cellsChangedThisClick) {
            toPrint = toPrint + entry.x + "," + entry.y + " -- ";
        }
        Debug.Log(toPrint);
    }

    public List<planeCoord> get() {
        return cellsChangedThisClick;
    }

    public void clear () {
        cellsChangedThisClick.Clear();
    }
}
