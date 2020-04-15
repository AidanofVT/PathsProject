using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                driver.GetComponent<BeatingHeart>().wallCells.Add(gameObject);
            }
            else {
                setAsNotWall();
                driver.GetComponent<BeatingHeart>().wallCells.Remove(gameObject);
            }
            string toPrint = null;
            foreach (GameObject entry in driver.GetComponent<BeatingHeart>().wallCells) {
                toPrint = toPrint + entry.GetComponent<SquareProperties>().nameInCoordinates.x + "," + entry.GetComponent<SquareProperties>().nameInCoordinates.y + " -- ";
            }
            //Debug.Log(toPrint);
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
        Debug.Log(nameInCoordinates.x + "," + nameInCoordinates.y + " isWall = " + (thisTile.Equals(tileType.isWall)));
        if (thisTile.Equals(tileType.isWall)) {
            return true;
        }
        return false;
    }
}
