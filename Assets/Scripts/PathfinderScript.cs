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
        List<planeCoord> excludedCells = new List<planeCoord>();
        public planeCoord startPoint;
        public planeCoord goalPoint;
        planeCoord workingCoordinate;
        int limiter = 0;
        int limit  = 2000;

        public void startUp(planeCoord start, planeCoord finish) {
                startPoint = start;
                goalPoint = finish;
                workingCoordinate = startPoint;
                pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                unexplored.Add(workingCoordinate);
        }

        public void search () {
                pokeAdjacentsToUnexplored();
                //Seems to not conform to the While parameter.
                while (unexplored.Count != 0 && workingCoordinate.compare(goalPoint) == false) {
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        excludedCells.Add(workingCoordinate);
                        logLists(); 
                        workingCoordinate = unexplored[0];
                        unexplored.Remove(workingCoordinate);
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        pokeAdjacentsToUnexplored();
                        loopBreaker("search");
                }
                Debug.Log("Search ended.");
        }

        void pokeAdjacentsToUnexplored () {
                for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                                if (i != 0 || j != 0) {
                                        try {
                                                if (isNewNavigable(pathfinderGrid[workingCoordinate.x + i, workingCoordinate.y + j]) == true) {
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

        bool isNewNavigable (GameObject maybeWall) {
                planeCoord trouble = maybeWall.GetComponent<SquareProperties>().nameInCoordinates;
                if (searchListForPlaneCoord(excludedCells, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
                        return false;
                }
                if (searchListForPlaneCoord(unexplored, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
                        return false;
                }
                else if (maybeWall.GetComponent<SquareProperties>().isWall() == true) {
                        excludedCells.Add(maybeWall.GetComponent<SquareProperties>().nameInCoordinates);
                        maybeWall.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                        return false;
                }
                return true;
        }

        bool searchListForPlaneCoord (List<planeCoord> toSearch, planeCoord term) {
                for (int i = 0; i <= toSearch.Count-1; i++) {
                        if (toSearch[i].x == term.x && toSearch[i].y == term.y) {
                                return true;
                        } 
                }
                return false;
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
                Debug.Log("Added " + toInsert.x + "," + toInsert.y + " to unexplored locations.");
                pathfinderGrid[toInsert.x, toInsert.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        void logLists () {
                string unexploredStatus = "";
                                foreach (planeCoord entry in unexplored) {
                                        unexploredStatus = unexploredStatus + entry.x + "," + entry.y + " -- ";
                                }
                        string excludedStatus = "";
                                foreach (planeCoord entry in excludedCells) {
                                        excludedStatus = excludedStatus + entry.x + "," + entry.y + " -- ";
                                }
                        Debug.Log("Finished poking around " + workingCoordinate.toInts() + " , the unexplored list is: " + unexploredStatus 
                        + ". The excludedCells list is: " + excludedStatus);
        }

        void loopBreaker (string problemArea) {
                limiter++;
                if (limiter == 500) {

                }
                if (limiter >= limit) {
                        throw new Exception("Infinite loop in " + problemArea + " function broken.");
                }
        }
}
