using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
        public GameObject driver;
        public GameObject[,] pathfinderGrid;
        List<planeCoord> unexplored = new List<planeCoord>();
        List<planeCoord> foundWalls = new List<planeCoord>();
        public planeCoord startPoint;
        public planeCoord goalPoint;
        planeCoord workingCoordinate;
        int limiter = 0;
        int limit  = 1000;

        public void startUp(planeCoord start, planeCoord finish) {
                startPoint = start;
                goalPoint = finish;
                workingCoordinate = startPoint;
                unexplored.Add(workingCoordinate);
                pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                //pokeAdjacentsToUnexplored();
        }

        public void search () {
                //int i = 0;
                while (unexplored.Count != 0 || workingCoordinate != goalPoint) {
                        planeCoord temp = workingCoordinate;
                        pokeAdjacentsToUnexplored();
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        unexplored.Remove(workingCoordinate);
                        workingCoordinate = unexplored[0];
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        string unexploredStatus = "";
                                foreach (planeCoord entry in unexplored) {
                                        unexploredStatus = unexploredStatus + entry.toInts() + " -- ";
                                }
                        Debug.Log("Finished poking around " + temp.toInts() + " , the unexplored list is: " + unexploredStatus);
                        // i++;
                        // if (i > 10) {
                        //         break;
                        // }
                        loopBreaker("search");
                }              
        }

        void pokeAdjacentsToUnexplored () {
                for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                                if (i != 0 || j != 0) {
                                        try {
                                                if (isNavigable(pathfinderGrid[workingCoordinate.x + i, workingCoordinate.y + j]) == true) {
                                                        planeCoord toInsert = new planeCoord(workingCoordinate.x + i, workingCoordinate.y + j);
                                                        insertUnexplored(toInsert);
                                                }
                                        }
                                        catch (System.IndexOutOfRangeException) {
                                                Debug.Log("Skipping an nonexistant location " + (workingCoordinate.x + i) + "," + (workingCoordinate.y + j));
                                        }                                        
                                }
                        }
                }
        }

        bool isNavigable (GameObject maybeWall) {
                if (maybeWall.GetComponent<SquareProperties>().isWall() == true) {
                        foundWalls.Add(maybeWall.GetComponent<SquareProperties>().nameInCoordinates);
                        maybeWall.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                        return false;
                }
                return true;
        }

        void insertUnexplored (planeCoord toInsert) {
                float key = toInsert.distanceTo(goalPoint);
                int min = 0;
                int max = unexplored.Count - 1;
                int mid = (min + max) / 2;
                while (min <= max) {
                        mid = (min + max) / 2;
                        float key2 = unexplored[mid].distanceTo(goalPoint);
                        if (key == unexplored[mid].distanceTo(goalPoint)) {  
                                break;  
                        }  
                        else if (key < unexplored[mid].distanceTo(goalPoint)) {  
                                max = mid - 1;  
                        }  
                        else {  
                                min = mid + 1;  
                        }
                        loopBreaker("insertUnexplored");
                }
                if (key < unexplored[mid].distanceTo(goalPoint)) {
                        unexplored.Insert(mid, toInsert);
                }
                else if (key > unexplored[mid].distanceTo(goalPoint)) {
                        unexplored.Insert(mid++, toInsert);
                }
                Debug.Log("Added " + toInsert.x + toInsert.y + " to unexplored locations.");
                pathfinderGrid[toInsert.x, toInsert.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        void loopBreaker (string problemArea) {
                limiter++;
                if (limiter >= limit) {
                        throw new Exception("Infinite loop in " + problemArea + " function broken.");
                }
        }
}
