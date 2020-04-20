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
        if (nextCar.currentTravelDirection == nextCar.nextCar.currentTravelDirection
        && nextCar.currentTravelDirection == clockWise(currentTravelDirection)
        && nextCar.nextCar.currentTravelDirection == clockWise(currentTravelDirection)) {
            Debug.Log("ClockWise turn identified.");
        }
        if (nextCar.currentTravelDirection == nextCar.nextCar.currentTravelDirection
        && nextCar.currentTravelDirection == counterClockWise(currentTravelDirection)
        && nextCar.nextCar.currentTravelDirection == counterClockWise(currentTravelDirection)) {
            Debug.Log("Counter-clockWise turn identified.");
        }
    }

}