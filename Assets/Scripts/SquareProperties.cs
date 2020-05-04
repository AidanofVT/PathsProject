﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareProperties : MonoBehaviour
{
    enum tileType {isWall, isNotWall, trainStart, cornerAdjacent};
    tileType thisTile = tileType.isNotWall;
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
    }

    public void nameCell (int x, int y) {
        this.nameInCoordinates = new planeCoord(x,y);
    }

    public void changeState(string newState = "toggleWall") {
        switch (newState) {
            case "toggleWall":
                if (clickedThisMouseDown == false) {
                    if (thisTile == tileType.isNotWall) {
                        setAsWall();
                    }
                    else if (thisTile == tileType.isWall) {
                        setAsNotWall();
                    }
                    driver.GetComponent<ClickedCellContainer>().add(nameInCoordinates);
                    clickedThisMouseDown = true;
                }
            break;
            case "cornerAdjacent":
                thisTile = tileType.cornerAdjacent;
            break;
            case "trainStart":
                thisTile = tileType.trainStart;
            break;
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
        if (thisTile.Equals(tileType.isWall)) {
            return true;
        }
        return false;
    }

    public string getState () {
        return thisTile.ToString();
    }

    public void mark (Vector2 wherePut, string toDisplay) {
        GameObject sign  = Instantiate(Textie, wherePut, Quaternion.identity);
        sign.transform.SetParent(canvas.transform);
        sign.GetComponent<Text>().text = toDisplay;
    }
}
