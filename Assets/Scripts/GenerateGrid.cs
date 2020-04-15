using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour {
    public GameObject cell;
    public Canvas canvas;

    public GameObject[,] generateGrid(int sideLength) {
        centerCamera(sideLength);
        GameObject[,] unfinishedGrid = new GameObject[sideLength,sideLength];
        int xItterator = 0;
        while (xItterator < sideLength) {
            int yItterator = 0;
            while (yItterator < sideLength) {
                Vector2 thisCoordinate = new Vector2 (xItterator * 20, yItterator * 20);
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
        sideLength = sideLength * 20;
        UnityEngine.Camera.main.transform.position = new Vector3(sideLength/2 - 0.5f, sideLength/2 - 0.5f, -1 * sideLength);
        UnityEngine.Camera.main.orthographicSize = sideLength * 2 / 3;
        canvas.GetComponent<Transform>().position = UnityEngine.Camera.main.transform.position;
    }
}

