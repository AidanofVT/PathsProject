using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareProperties : MonoBehaviour
{
    enum tileType {isWall, isNotWall};
    tileType thisTile = tileType.isNotWall;
    public GameObject routeParent = null;
    public bool clickedThisMouseDown = false;
    public planeCoord nameInCoordinates;
    public GameObject driver;

    public void nameCell (int x, int y) {
        this.nameInCoordinates = new planeCoord(x,y);
    }

    public void changeState() {
        if (clickedThisMouseDown == false) {
            if (thisTile == tileType.isNotWall) {
                setAsWall();
            }
            else {
                setAsNotWall();
            }
            clickedThisMouseDown = true;
            driver.GetComponent<ClickedCellContainer>().add(nameInCoordinates);
        }
    }

    void setAsWall (){
        thisTile = tileType.isWall;
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
    }

    void setAsNotWall () {
        thisTile = tileType.isNotWall;
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    public bool isWall () {
        if (thisTile == tileType.isWall) {
            return true;
        }
        return false;
    }
}
