using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareProperties : MonoBehaviour
{
    List<string> thisTile = new List<string>();
    //enum tileType {isWall, isNotWall, trainStart, cornerAdjacent, terminus};
    //tileType thisTile = tileType.isNotWall;
    public GameObject routeParent = null;
    public List<GameObject> pathToParent = null;
    public int pathLengthFromStart;
    public bool clickedThisMouseDown = false;
    public planeCoord nameInCoordinates;
    public GameObject driver;
    public Obstacle greatWall;
    Canvas canvas;
    public GameObject Textie;

    private void Start() {
        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        thisTile.Add("notWall");
    }

    public void nameCell (int x, int y) {
        this.nameInCoordinates = new planeCoord(x,y);
    }

    public void addState (string newState) {
        if (newState == "wall" || newState == "notWall") {
            if (clickedThisMouseDown == false) {
                if (thisTile.Contains("notWall")) {
                    setAsWall();
                }
                else if (thisTile.Contains("wall")) {
                    setAsNotWall();
                }
                driver.GetComponent<ClickedCellContainer>().add(nameInCoordinates);
                clickedThisMouseDown = true;
            }
        }
        else {
            thisTile.Add(newState);
        }
    }

    // public void changeState(string newState = "toggleWall") {
    //     switch (newState) {
    //         case "toggleWall":
    //             if (clickedThisMouseDown == false) {
    //                 if (thisTile == tileType.isNotWall) {
    //                     setAsWall();
    //                 }
    //                 else if (thisTile == tileType.isWall) {
    //                     setAsNotWall();
    //                 }
    //                 driver.GetComponent<ClickedCellContainer>().add(nameInCoordinates);
    //                 clickedThisMouseDown = true;
    //             }
    //         break;
    //         case "cornerAdjacent":
    //             thisTile = tileType.cornerAdjacent;
    //         break;
    //         case "trainStart":
    //             thisTile = tileType.trainStart;
    //         break;
    //         case "terminus":
    //             thisTile = tileType.terminus;
    //         break;
    //     }
    // }

    void setAsWall (){
        thisTile.Add("wall");
        thisTile.Remove("notWall");
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
    }

    void setAsNotWall () {
        thisTile.Add("notWall");
        thisTile.Remove("wall");
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    public bool isA (string query) {
        if (thisTile.Contains(query)) {
            return true;
        }
        return false;
    }

    public List<string> getState () {
        return thisTile;
    }

    public void mark (Vector2 wherePut, string toDisplay) {
        GameObject sign  = Instantiate(Textie, wherePut, Quaternion.identity);
        sign.transform.SetParent(canvas.transform);
        sign.GetComponent<Text>().text = toDisplay;
    }
}
