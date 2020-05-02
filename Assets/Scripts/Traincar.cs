using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traincar : MonoBehaviour {
    public enum direction {north, east, south, west, error};
    public direction currentTravelDirection = direction.north;
    public planeCoord currentCoord;
    public Traincar nextCar;
    public Traincar priorCar;
    public Train parentTrain;
    public Canvas canvas;
    public GameObject Textie;
    GameObject[,] map = GameObject.Find("Driver").GetComponent<BeatingHeart>().grid;
    GameObject localCarSign;
    int limit = 2000;
    int limiter = 0;

    public Traincar (planeCoord startingCoord, GameObject carSign) {
        currentCoord = startingCoord;
        localCarSign = carSign;
    }

    public void advance () {
        //Debug.Log("Commencing move from " + currentCoord.x + "," + currentCoord.y);
        currentTravelDirection = counterClockWise(currentTravelDirection);
        while (isNavigableWall(adjacentCell(currentTravelDirection)) == false) { 
            currentTravelDirection = clockWise(currentTravelDirection);
            loopBreaker("Traincar advance");
        }
        if (adjacentCell(currentTravelDirection) == null) {

        }
        localCarSign.GetComponent<Transform>().position = adjacentCell(currentTravelDirection).GetComponent<Transform>().position;
        currentCoord = adjacentCell(currentTravelDirection).GetComponent<SquareProperties>().nameInCoordinates;
        //Debug.Log("Concluded move to " + currentCoord.x + "," + currentCoord.y);
    }

    protected direction clockWise (direction current) {
        switch (current) {
            case direction.north:
                return direction.east;
            break;
            case direction.east:
                return direction.south;
            break;
            case direction.south:
                return direction.west;
            break;
            case direction.west:
                return direction.north;
        }
        return direction.error;
    }

    protected direction counterClockWise (direction current) {
        switch (current) {
            case direction.north:
                return direction.west;
            break;
            case direction.east:
                return direction.north;
            break;
            case direction.south:
                return direction.east;
            break;
            case direction.west:
                return direction.south;
        }
        return direction.error;
    }

    bool isNavigableWall (GameObject maybeNavigable) {
        try {
            if (maybeNavigable.GetComponent<SquareProperties>().getState() == "isWall"
            || maybeNavigable.GetComponent<SquareProperties>().getState() == "trainStart") {
                return true;
            }
        }
        catch (System.NullReferenceException) {
        }
        return false;
    }

    protected GameObject adjacentCell (direction inDirection, bool plusFourtyFive = false) {
        try {
            switch (inDirection) {
                case direction.north:
                    if (plusFourtyFive == true) {
                        return map[currentCoord.x + 1, currentCoord.y + 1];
                    }
                    return map[currentCoord.x, currentCoord.y + 1];
                break;
                case direction.east:
                    if (plusFourtyFive == true) {
                        return map[currentCoord.x + 1, currentCoord.y - 1];
                    }
                    return map[currentCoord.x + 1, currentCoord.y];
                break;
                case direction.south:
                    if (plusFourtyFive == true) {
                        return map[currentCoord.x - 1, currentCoord.y - 1];
                    }                
                    return map[currentCoord.x, currentCoord.y - 1];
                break;
                case direction.west:
                    if (plusFourtyFive == true) {
                        return map[currentCoord.x - 1, currentCoord.y + 1];
                    }
                    return map[currentCoord.x - 1, currentCoord.y];
                break;
            }
        }
        catch (System.IndexOutOfRangeException) {
        }
        return null;
    }

    protected direction relativePosition (planeCoord otherSquare) {
        if (otherSquare.x == currentCoord.x && otherSquare.y >= currentCoord.y) {
            return direction.north;
        }
        else if (otherSquare.x == currentCoord.x && otherSquare.y <= currentCoord.y) {
            return direction.south;
        }
        else if (otherSquare.y == currentCoord.y && otherSquare.x >= currentCoord.x) {
            return direction.east;
        }
        else if (otherSquare.y == currentCoord.y && otherSquare.x <= currentCoord.x) {
            return direction.west;
        }
        return direction.error;
    }

    public GameObject getSquare () {
        return map[currentCoord.x, currentCoord.y];
    }

    public void destroyCarSign () {
        Destroy(localCarSign);
    }

    void loopBreaker (string problemArea) {
        limiter++;
        if (limiter >= limit) {
                throw new Exception("Infinite loop in " + problemArea + " function broken.");
        }
    }
}
