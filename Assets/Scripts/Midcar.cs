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
        Debug.Log("Commencing scan from " + currentCoord.x + "," + currentCoord.y);
        if (relativePosition(nextCar.currentCoord) == relativePosition(nextCar.nextCar.currentCoord)
        && relativePosition(nextCar.currentCoord) == clockWise(clockWise(currentTravelDirection))
        && relativePosition(priorCar.currentCoord) == relativePosition(priorCar.priorCar.currentCoord)) {
            if (relativePosition(priorCar.currentCoord) == clockWise(currentTravelDirection)) {
                Debug.Log("Clockwise turn identified.");
                GameObject toTag = adjacentCell(counterClockWise(currentTravelDirection), true);
                tagSquare(toTag, "C", "isNotWall");
                parentTrain.currentTrack.associatedCorners.Add(toTag, "clockwise");
            }
            else if (relativePosition(priorCar.currentCoord) == counterClockWise(currentTravelDirection)) {
                Debug.Log("Counter-clockwise turn identified.");
                GameObject toTag = adjacentCell(currentTravelDirection, true);
                tagSquare(toTag, "C", "isNotWall");
                parentTrain.currentTrack.associatedCorners.Add(toTag, "counter-clockwise");
            }
            else if (relativePosition(priorCar.currentCoord) == clockWise(clockWise(currentTravelDirection))) {
                Debug.Log("About-face turn identified.");
                GameObject toTag = adjacentCell(currentTravelDirection);
                tagSquare(toTag, "C", "isNotWall");
                parentTrain.currentTrack.associatedCorners.Add(toTag, null);
            }
        }
    }

    void tagSquare (GameObject toTag, string tagMessage, string requiredState) {
        if (toTag.GetComponent<SquareProperties>().getState() == requiredState) {
            Vector2 targetSquarePos = toTag.GetComponent<Transform>().position;
            toTag.GetComponent<SquareProperties>().mark(targetSquarePos, tagMessage);
        }
    }
}