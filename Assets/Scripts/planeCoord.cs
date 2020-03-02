using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeCoord
{
    public int x;
    public int y;

    public planeCoord (int x, int y) {
        this.x = x;
        this.y = y;
    }

    public (int,int) toInts () {
        return (x, y);
    }

    public float distanceTo (planeCoord from) {
        return Mathf.Abs((Mathf.Sqrt(Mathf.Pow(from.x - x, 2) + Mathf.Pow(from.y -y, 2))));
    }

    public int course (planeCoord from) {
        return (y - from.y) / (x - from.x);
    }
}

    // public int x () {
    //     return xCoordinate;
    // }

    // public int y () {
    //     return yCoordinate;
    // }