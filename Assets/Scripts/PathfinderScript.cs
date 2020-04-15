using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
        public GameObject[,] pathfinderGrid;
        List<planeCoord> unPoked = new List<planeCoord>();
        List<planeCoord> excludedCells = new List<planeCoord>();
        public planeCoord startPoint;
        public planeCoord goalPoint;
        planeCoord workingCoordinate;
        int limiter = 0;
        int limit  = 2000;

        public void startUp(planeCoord start, planeCoord finish) {
                startPoint = start;
                goalPoint = finish;
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
                Debug.Log("Search ended.");
                markPath(pathfinderGrid[workingCoordinate.x, workingCoordinate.y]);
        }

        void changeWorkingCoordinate (planeCoord newWork) {
                if (workingCoordinate != null) {
                        pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                        excludedCells.Add(workingCoordinate);
                        //pathfinderGrid[newWork.x, newWork.y].GetComponent<SquareProperties>().routeParent = pathfinderGrid[workingCoordinate.x, workingCoordinate.y];
                }
                workingCoordinate = newWork;
                pathfinderGrid[workingCoordinate.x, workingCoordinate.y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        void pokeAdjacentsToUnexplored () {
                unPoked.Remove(workingCoordinate);
                for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                                if (i != 0 || j != 0) {
                                        try {
                                                if (isNewNavigable(pathfinderGrid[workingCoordinate.x + i, workingCoordinate.y + j]) == true) {
                                                        planeCoord toInsert = new planeCoord(workingCoordinate.x + i, workingCoordinate.y + j);
                                                        insertUnpoked(toInsert);
                                                        pathfinderGrid[toInsert.x, toInsert.y].GetComponent<SquareProperties>().routeParent = pathfinderGrid[workingCoordinate.x, workingCoordinate.y];
                                                }
                                        }
                                        catch (System.IndexOutOfRangeException) {
                                                Debug.Log("Skipping an nonexistant location " + (workingCoordinate.x + i) + "," + (workingCoordinate.y + j));
                                        }                                        
                                }
                        }
                }
                logLists();

        }

        bool isNewNavigable (GameObject maybeWall) {
                planeCoord trouble = maybeWall.GetComponent<SquareProperties>().nameInCoordinates;
                if (searchListForPlaneCoord(excludedCells, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
                        return false;
                }
                if (searchListForPlaneCoord(unPoked, maybeWall.GetComponent<SquareProperties>().nameInCoordinates)) {
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

        void insertUnpoked (planeCoord toInsert) {
                float newAppeal = toInsert.distanceTo(goalPoint);
                float midAppeal = 0.0f;
                int min = 0;
                int max = unPoked.Count - 1;
                int mid = (min + max) / 2;
                while (min <= max) {
                        mid = (min + max) / 2;
                        midAppeal = unPoked[mid].distanceTo(goalPoint);
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
                pathfinderGrid[toInsert.x, toInsert.y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }

        void markPath (GameObject square) {
                try {
                        if (square.GetComponent<SquareProperties>() == null && square != pathfinderGrid[startPoint.x, startPoint.y]) {
                                throw new Exception("Attempted to mark a path from a square with no routeParent: " + square.GetComponent<SquareProperties>().nameInCoordinates.toInts());
                        }
                }
                catch (UnassignedReferenceException) {                        
                }
                square.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                markPath (square.GetComponent<SquareProperties>().routeParent);
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
