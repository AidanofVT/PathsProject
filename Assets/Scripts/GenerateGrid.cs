using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour {
    public GameObject cell;

    public GameObject[,] generateGrid(int sideLength) {
        centerCamera(sideLength);
        GameObject[,] unfinishedGrid = new GameObject[sideLength,sideLength];
        int xItterator = 0;
        while (xItterator < sideLength) {
            int yItterator = 0;
            while (yItterator < sideLength) {
                Vector2 thisCoordinate = new Vector2 (xItterator, yItterator);
                unfinishedGrid [xItterator, yItterator] = Instantiate(cell, thisCoordinate, Quaternion.identity);
                unfinishedGrid [xItterator, yItterator].GetComponent<SquareProperties>().nameCell(xItterator, yItterator);
                unfinishedGrid [xItterator, yItterator].GetComponent<SquareProperties>().driver = gameObject;
                yItterator++;
            }
            xItterator++;
        }
        return unfinishedGrid;
    }

    void centerCamera(int sideLength) {
        UnityEngine.Camera.main.transform.position = new Vector3(sideLength/2 - 0.5f, sideLength/2 - 0.5f, -1 * sideLength);
        UnityEngine.Camera.main.orthographicSize = sideLength * 2 / 3;
    }
}
    // void generateWalls() {
    //     int[] topLeft = {-1 * sideLength, sideLength};
    //     growWalls(topLeft, 1, 0);
    // }

    // void growWalls (int[] startingCorner, int xLeftOrRight, int yUpOrDown) {
    //     foreach (int moveAlong in generateWallStartPoints()) {
    //         Debug.Log("starting corner X = " + startingCorner[0] + "... starting corner y = " + startingCorner[1] + "... " + "moveAlong = " + moveAlong
    //          + "... xLeftOrRight = " + xLeftOrRight + "... yUpOrDown = " + yUpOrDown);
    //         grid[startingCorner[0] + sideLength + moveAlong * xLeftOrRight, startingCorner[1] + moveAlong * yUpOrDown].GetComponent<SquareProperties>().changeState();
    //     }
    // }

    // List<int> generateWallStartPoints() {
    //     List<int> starts = new List<int>();
    //         for (int numberOfStarts = (int)(Mathf.Sqrt(UnityEngine.Random.Range(0.0f, 9.0f))); numberOfStarts > 0; numberOfStarts--) {
    //             starts.Add(UnityEngine.Random.Range(0,30));
    //         }
    //     return starts;
    // }

