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
            GameObject targetSquare = null;
            if (relativePosition(priorCar.currentCoord) == clockWise(currentTravelDirection)) {
                Debug.Log("Clockwise turn identified.");
                targetSquare = adjacentCell(counterClockWise(currentTravelDirection), true);
            }
            else if (relativePosition(priorCar.currentCoord) == counterClockWise(currentTravelDirection)) {
                Debug.Log("Counter-clockwise turn identified.");
                targetSquare = adjacentCell(currentTravelDirection, true);
            }
            else if (relativePosition(priorCar.currentCoord) == clockWise(clockWise(currentTravelDirection))) {
                Debug.Log("About-face turn identified.");
                targetSquare = adjacentCell(currentTravelDirection);
            }
            tagSquare(targetSquare, "C", "isNotWall");
        }
    }

    void tagSquare (GameObject toTag, string tagMessage, string requiredState) {
        if (toTag.GetComponent<SquareProperties>().getState() == requiredState) {
            Vector2 targetSquarePos = toTag.GetComponent<Transform>().position;
            toTag.GetComponent<SquareProperties>().mark(targetSquarePos, tagMessage);
        }
    }
}