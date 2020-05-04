using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerPathfinder : MonoBehaviour {
        public GameObject[,] pathfinderGrid;
        List<planeCoord> unPoked = new List<planeCoord>();
        List<planeCoord> excludedCells = new List<planeCoord>();
        ObstacleStopAStar aStarPathfinder;
        // Hashtable listOfObstacles = GameObject.Find("Driver").GetComponent<BeatingHeart>().identifiedObstacles;
        planeCoord startPoint;
        planeCoord goalPoint;
        planeCoord workingCoordinate;
        public char currentBlocker = '-';
        int limiter = 0;
        int limit  = 2000;

        public void startUp(planeCoord start, planeCoord finish) {
                startPoint = start;
                goalPoint = finish;
                aStarPathfinder = new ObstacleStopAStar(this);
                aStarPathfinder.pathfinderGrid = pathfinderGrid;
                changeWorkingCoordinate (startPoint);
                unPoked.Add(workingCoordinate);
        }

        public void search () {
                pokeAdjacentsToUnexplored();
                unPoked.Remove(startPoint);
                while (unPoked.Count != 0 && workingCoordinate.compare(goalPoint) == false) {
                        changeWorkingCoordinate(unPoked[0]);
                        pokeAdjacentsToUnexplored();
                        loopBreaker("search");
                }
                Debug.Log("Cornered A* search ended.");
        }

        void changeWorkingCoordinate (planeCoord newWork) {
                if (workingCoordinate != null) {
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        excludedCells.Add(workingCoordinate);
                }
                workingCoordinate = newWork;
                pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        void pokeAdjacentsToUnexplored () {
                planeCoord mostClockWise = null;
                planeCoord mostCounterClockWise = null;
                List<GameObject> nullMeansBlocked = aStarPathfinder.search(workingCoordinate, goalPoint);
                if (nullMeansBlocked == null) {
                        aStarPathfinder.ignoredObstacle = currentBlocker;
                        Obstacle withCorners = (Obstacle)GameObject.Find("Driver").GetComponent<BeatingHeart>().identifiedObstacles[currentBlocker];
                        List<planeCoord> candidatecorners = withCorners.associatedCorners;
                        mostClockWise = candidatecorners[0];
                        mostCounterClockWise = candidatecorners[0];
                        foreach (planeCoord maybeExtreme in candidatecorners) {
                                if (maybeExtreme.course(workingCoordinate) < mostClockWise.course(workingCoordinate)) {
                                        mostClockWise = maybeExtreme;
                                }
                                if (maybeExtreme.course(workingCoordinate) > mostCounterClockWise.course(workingCoordinate)) {
                                        mostCounterClockWise = maybeExtreme;
                                }
                        }
                }
                else {
                        workingCoordinate = goalPoint;
                        return;
                }
                unPoked.Remove(workingCoordinate);
                pathfinderGrid[mostClockWise.x, mostClockWise.y].GetComponent<SquareProperties>().pathToParent = aStarPathfinder.search(mostClockWise, workingCoordinate);
                pathfinderGrid[mostCounterClockWise.x, mostCounterClockWise.y].GetComponent<SquareProperties>().pathToParent = aStarPathfinder.search(mostCounterClockWise, workingCoordinate);
                pathfinderGrid[mostClockWise.x, mostClockWise.y].GetComponent<SquareProperties>().routeParent = pathfinderGrid[workingCoordinate.x, workingCoordinate.y];
                pathfinderGrid[mostCounterClockWise.x, mostCounterClockWise.y].GetComponent<SquareProperties>().routeParent = pathfinderGrid[workingCoordinate.x, workingCoordinate.y];
                GameObject testParent = pathfinderGrid[mostClockWise.x, mostClockWise.y].GetComponent<SquareProperties>().routeParent;
                List<GameObject> testPath = pathfinderGrid[mostClockWise.x, mostClockWise.y].GetComponent<SquareProperties>().pathToParent;
                insertUnpoked(mostClockWise);
                insertUnpoked(mostCounterClockWise);
                currentBlocker = '-';
        }

        void insertUnpoked (planeCoord toInsert) {
                int newAppeal = compileRoute(pathfinderGrid[toInsert.x, toInsert.y]).Count;
                int midAppeal = 0;
                int min = 0;
                int max = unPoked.Count - 1;
                int mid = (min + max) / 2;
                while (min <= max) {
                        mid = (min + max) / 2;
                        midAppeal = pathfinderGrid[unPoked[mid].x, unPoked[mid].y].GetComponent<SquareProperties>().pathLengthFromStart;
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
                Debug.Log("(Cornered A*) Added " + toInsert.x + "," + toInsert.y + " to unexplored locations.");
                pathfinderGrid[toInsert.x, toInsert.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        List<GameObject> compileRoute (GameObject square) {
                GameObject originalSquare = square;
                List<GameObject> backwardsRoute = new List<GameObject>();
                GameObject testParent = square.GetComponent<SquareProperties>().routeParent;
                List<GameObject> testPath = square.GetComponent<SquareProperties>().pathToParent;
                while (square.GetComponent<SquareProperties>().routeParent != null) {
                        backwardsRoute.AddRange(square.GetComponent<SquareProperties>().pathToParent);
                        square = square.GetComponent<SquareProperties>().routeParent;
                        loopBreaker("compileRoute (stuck on " + square.GetComponent<SquareProperties>().nameInCoordinates.x + "," + square.GetComponent<SquareProperties>().nameInCoordinates.y + ")");
                }
                originalSquare.GetComponent<SquareProperties>().pathLengthFromStart = backwardsRoute.Count;
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
