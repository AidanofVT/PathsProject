﻿using System.Collections;
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

    public float course (planeCoord from) {
        return Mathf.Atan2(y - from.y, x - from.x);
    }

    public bool compare(planeCoord p) {
        if (p == null) {
            return false;
        }
        else if (p.x == this.x && p.y == this.y) {
            return true;
        }
        return false;
    }
}