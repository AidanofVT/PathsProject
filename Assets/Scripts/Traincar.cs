using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traincar {
    public enum direction {north, east, south, west, error};
    direction currentTravelDirection = direction.north;
    GameObject[,] map = GameObject.Find("Driver").GetComponent<BeatingHeart>().grid;
    public planeCoord currentCoord;

    public Traincar (planeCoord startingCoord) {
        currentCoord = startingCoord;
    }

    public void advance () {
        currentTravelDirection = counterClockWise(counterClockWise(currentTravelDirection));
        while (adjacentCell(currentTravelDirection).GetComponent<SquareProperties>().isWall() == false) {
            currentTravelDirection = counterClockWise(currentTravelDirection);
        }
        map[currentCoord.x, currentCoord.y].GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
        currentCoord = adjacentCell(currentTravelDirection).GetComponent<SquareProperties>().nameInCoordinates;
        map[currentCoord.x, currentCoord.y].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }

    direction clockWise (direction current) {
        switch (current) {
            case direction.north:
                return direction.east;
            break;
            case direction.east:
                return direction.south;
            break;
            case direction.south:
                return direction.west;
            break;
            case direction.west:
                return direction.north;
        }
        return direction.error;
    }

    direction counterClockWise (direction current) {
        switch (current) {
            case direction.north:
                return direction.west;
            break;
            case direction.east:
                return direction.north;
            break;
            case direction.south:
                return direction.east;
            break;
            case direction.west:
                return direction.south;
        }
        return direction.error;
    }

    GameObject adjacentCell (direction inDirection) {
        try {
            switch (inDirection) {
                case direction.north:
                    return map[currentCoord.x, currentCoord.y + 1];
                break;
                case direction.east:
                    return map[currentCoord.x + 1, currentCoord.y];
                break;
                case direction.south:
                    return map[currentCoord.x, currentCoord.y - 1];
                break;
                case direction.west:
                    return map[currentCoord.x, currentCoord.y - 1];
                break;
            }
        }
        catch (System.IndexOutOfRangeException) {
        }
        return null;
    }
}
