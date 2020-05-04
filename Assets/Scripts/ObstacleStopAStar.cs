using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStopAStar : MonoBehaviour {
        CornerPathfinder parentPathfinder;
        public GameObject[,] pathfinderGrid;
        List<planeCoord> unPoked = new List<planeCoord>();
        List<planeCoord> excludedCells = new List<planeCoord>();
        List<GameObject> squaresWithParents = new List<GameObject>();
        public planeCoord startPoint;
        public planeCoord goalPoint;
        planeCoord workingCoordinate;
        public char ignoredObstacle = '-';
        bool go = false;
        int limiter = 0;
        int limit  = 2000;

        public ObstacleStopAStar(CornerPathfinder parent) {
                parentPathfinder = parent;
        }

        private void FixedUpdate() {
                if (unPoked.Count != 0 && workingCoordinate.compare(goalPoint) == false) {

                }
        }

        public List<GameObject> search (planeCoord start, planeCoord goal) {
                startPoint = start;
                goalPoint = goal;
                changeWorkingCoordinate(startPoint);
                unPoked.Add(workingCoordinate);
                pokeAdjacentsToUnexplored();
                unPoked.Remove(startPoint);
                while (unPoked.Count != 0 && workingCoordinate.compare(goalPoint) == false) {
                        changeWorkingCoordinate(unPoked[0]);
                        parentPathfinder.currentBlocker = pokeAdjacentsToUnexplored();
                        if (parentPathfinder.currentBlocker != '-') {
                                zeroOut();
                                Debug.Log("Obstacle-stop A* search terminated; obstacle encountered.");
                                return null;
                        }
                        loopBreaker("search");
                }
                Debug.Log("Obstacle-stop A* search ended.");
                List<GameObject> toReturn = compileRoute(pathfinderGrid[workingCoordinate.x, workingCoordinate.y]);
                zeroOut();
                return toReturn;
        }

        void zeroOut () {
                limiter = 0;
                unPoked.Clear();
                excludedCells.Clear();
                workingCoordinate = null;
                foreach (GameObject touched in squaresWithParents) {
                        touched.GetComponent<SquareProperties>().routeParent = null;
                }
        }

        void changeWorkingCoordinate (planeCoord newWork) {
                if (workingCoordinate != null) {
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                        excludedCells.Add(workingCoordinate);
                }
                workingCoordinate = newWork;
                pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        char pokeAdjacentsToUnexplored () {
                unPoked.Remove(workingCoordinate);
                for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                                if (i != 0 || j != 0) {
                                        try {
                                                char navTypChar = isNewNavigable(pathfinderGrid[workingCoordinate.x + i, workingCoordinate.y + j]);
                                                if (navTypChar == '1') {
                                                        planeCoord toInsert = new planeCoord(workingCoordinate.x + i, workingCoordinate.y + j);
                                                        insertUnpoked(toInsert);
                                                        pathfinderGrid[toInsert.x, toInsert.y].GetComponent<SquareProperties>().routeParent = pathfinderGrid[workingCoordinate.x, workingCoordinate.y];
                                                        squaresWithParents.Add(pathfinderGrid[toInsert.x, toInsert.y]);
                                                        pathfinderGrid[toInsert.x, toInsert.y].GetComponent<SquareProperties>().pathLengthFromStart = compileRoute(pathfinderGrid[toInsert.x, toInsert.y]).Count;
                                                }
                                                else if (navTypChar != ignoredObstacle && navTypChar != '0') {                                                        
                                                        return navTypChar;
                                                } 
                                        }
                                        catch (System.IndexOutOfRangeException) {
                                                Debug.Log("Skipping an nonexistant location " + (workingCoordinate.x + i) + "," + (workingCoordinate.y + j));
                                        }                                        
                                }
                        }
                }
                logLists();
                return '-';
        }

        char isNewNavigable (GameObject maybeWall) {
                if (searchListForPlaneCoord(excludedCells, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
                        return '0';
                }
                if (searchListForPlaneCoord(unPoked, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
                        return '0';
                }
                else if (maybeWall.GetComponent<SquareProperties>().getState() == "isWall"
                        || maybeWall.GetComponent<SquareProperties>().getState() == "trainStart") {
                        if (maybeWall.GetComponent<SquareProperties>().greatWall != null) {
                                return maybeWall.GetComponent<SquareProperties>().greatWall.nameChar;
                        }
                        excludedCells.Add(maybeWall.GetComponent<SquareProperties>().nameInCoordinates);
                        maybeWall.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                        return '0';
                }
                return '1';
        }

        bool searchListForPlaneCoord (List<planeCoord> toSearch, planeCoord term) {
                for (int i = 0; i <= toSearch.Count-1; i++) {
                        if (toSearch[i].x == term.x && toSearch[i].y == term.y) {
                                return true;
                        } 
                }
                return false;
        }

        void insertUnpoked (planeCoord toInsert) {
                float testToStart = pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<SquareProperties>().pathLengthFromStart;
                float testtoFinish = toInsert.distanceTo(goalPoint);
                float testTeNext = workingCoordinate.distanceTo(toInsert);
                float newAppeal = pathfinderGrid[toInsert.x, toInsert.y].GetComponent<SquareProperties>().pathLengthFromStart
                                         + workingCoordinate.distanceTo(toInsert) + toInsert.distanceTo(goalPoint);
                float midAppeal = 0.0f;
                int min = 0;
                int max = unPoked.Count - 1;
                int mid = (min + max) / 2;
                while (min <= max) {
                        mid = (min + max) / 2;
                        midAppeal = pathfinderGrid[unPoked[mid].x, unPoked[mid].y].GetComponent<SquareProperties>().pathLengthFromStart
                                         + unPoked[mid].distanceTo(goalPoint);
                        if  (newAppeal == midAppeal) {  
                                break;
                        }  
                        else if  (newAppeal < midAppeal) {  
                                max = mid - 1;  
                        }  
                        else {  
                                min = mid + 1;  
                        }
                        loopBreaker("insertUnexplored");
                }
                if  (newAppeal < midAppeal) {
                        unPoked.Insert(mid, toInsert);
                }
                else {
                        unPoked.Insert(mid++, toInsert);
                }
                Debug.Log("Added " + toInsert.x + "," + toInsert.y + " to unexplored locations.");
                //pathfinderGrid[toInsert.x, toInsert.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        List<GameObject> compileRoute (GameObject square) {
                List<GameObject> backwardsRoute = new List<GameObject>();
                while (square.GetComponent<SquareProperties>().routeParent != null) {
                        if (square.GetComponent<SquareProperties>().routeParent.GetComponent<SquareProperties>().routeParent = square) {
                                break;
                        }
                        backwardsRoute.Add(square);
                        square = square.GetComponent<SquareProperties>().routeParent;
                        loopBreaker("compileRoute (stuck on " + square.GetComponent<SquareProperties>().nameInCoordinates.x + "," + square.GetComponent<SquareProperties>().nameInCoordinates.y + ")");
                }
                backwardsRoute.Add(square);
                return backwardsRoute;
        }

        void markPath (List<planeCoord> toMark) {
                foreach (planeCoord routeMember in toMark) {
                        pathfinderGrid[routeMember.x, routeMember.y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                } 
        } 

        void logLists () {
                string unexploredStatus = "";
                                foreach (planeCoord entry in unPoked) {
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
                if (limiter >= limit) {
                        throw new Exception("Infinite loop in " + problemArea + " function broken.");
                }
        }
}
