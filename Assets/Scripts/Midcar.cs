using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Midcar : Traincar {
    GameObject[,] map = GameObject.Find("Driver").GetComponent<BeatingHeart>().grid;
    GameObject localCarSign;
    int limit = 500;
    int limiter = 0;

    public Midcar (planeCoord startingCoord, GameObject carSign) : base (startingCoord, carSign) {
        currentCoord = startingCoord;
        localCarSign = carSign;
    }

    public void scan () {
        Debug.Log("Commencing scan from " + currentCoord.x + "," + currentCoord.y);
        checkForCorner();
        Debug.Log("Concluded scan from " + currentCoord.x + "," + currentCoord.y);
    }

    void checkForCorner() {
        if (relativePosition(nextCar.currentCoord) == relativePosition(nextCar.nextCar.currentCoord)
        && relativePosition(nextCar.currentCoord) == clockWise(clockWise(currentTravelDirection))
        && relativePosition(priorCar.currentCoord) == relativePosition(priorCar.priorCar.currentCoord)) {
            if (relativePosition(priorCar.currentCoord) == clockWise(currentTravelDirection)) {
                Debug.Log("Clockwise turn identified.");
                GameObject toTag = adjacentCell(counterClockWise(currentTravelDirection), true);
                tagCornerSquare(toTag, "isNotWall", "clockwise");
            }
            else if (relativePosition(priorCar.currentCoord) == counterClockWise(currentTravelDirection)) {
                Debug.Log("Counter-clockwise turn identified.");
                GameObject toTag = adjacentCell(currentTravelDirection, true);
                tagCornerSquare(toTag, "isNotWall", "counter-clockwise");
            }
            else if (relativePosition(priorCar.currentCoord) == clockWise(clockWise(currentTravelDirection))) {
                Debug.Log("About-face turn identified.");
                GameObject toTag = adjacentCell(currentTravelDirection);
                tagCornerSquare(toTag, "isNotWall");
            }
        }
    }

    void tagCornerSquare (GameObject toTag, string requiredState, string newSquareState = null) {
        try {
            if (toTag.GetComponent<SquareProperties>().getState() == requiredState) {
                Vector2 targetSquarePos = toTag.GetComponent<Transform>().position;
                toTag.GetComponent<SquareProperties>().mark(targetSquarePos, "C");
                planeCoord toTagCoord = toTag.GetComponent<SquareProperties>().nameInCoordinates;
                // if (parentTrain.currentTrack.associatedCorners.ContainsKey(toTag) == false) {
                //     parentTrain.currentTrack.associatedCorners.Add(toTag, newSquareState);
                // }
                if (parentTrain.currentTrack.associatedCorners.Contains(toTagCoord) == false) {
                    parentTrain.currentTrack.associatedCorners.Add(toTagCoord);
                }
            }
        }
        catch (System.NullReferenceException) {
        }
    }
}