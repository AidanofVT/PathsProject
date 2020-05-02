using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle {
    public List<GameObject> constituentWalls = new List<GameObject>(0);
    public Hashtable associatedCorners = new Hashtable();
    public char nameChar;
    bool isLoop;

    public int size () {
        return constituentWalls.Count;
    }
}