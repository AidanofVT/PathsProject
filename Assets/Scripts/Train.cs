using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour {
    List<Traincar> cars = new List<Traincar>(5);
    public List<Obstacle> obstacles = new List<Obstacle>();
    public Obstacle currentTrack = null;
    public GameObject driver;
    List<GameObject> walls;
    public Canvas canvas;
    public GameObject Textie;
    char[] obsticalNamesArray = {'Z','Y','X','W','V','U','T','S'};
    List<char> obsticalNames;
    int limit = 5000;
    int limiter = 0;
    bool go = false;

    private void Awake() {
        walls = driver.GetComponent<BeatingHeart>().wallCells;
        obsticalNames = new List<char>(obsticalNamesArray);
    }

    public void conduct () {
        chooChoo();
        while (walls.Count > 0) {
            advance();
            loopBreaker("Conduct");
        }
        endTrack();
        Debug.Log("Finished crawling.");
        Hashtable namesToObstacles = new Hashtable();
        foreach (Obstacle hunk in obstacles) {
            namesToObstacles.Add(hunk.nameChar, hunk);
        }
        GameObject.Find("Driver").GetComponent<BeatingHeart>().identifiedObstacles = namesToObstacles;
    }

    public void chooChoo () {
        //Time.fixedDeltaTime = 0.7f;
        planeCoord startPoint = walls[0].GetComponent<SquareProperties>().nameInCoordinates;
        Vector2 signStart = walls[0].GetComponent<Transform>().position;
        for (int i = 1; i <= 5; i++) {
            if (i == 3) {
                Midcar newCar = new Midcar(startPoint, mark(signStart, i.ToString()));
                newCar.parentTrain = this;
                cars.Add(newCar);
            }
            else {
                Traincar newCar = new Traincar(startPoint, mark(signStart, i.ToString()));
                newCar.parentTrain = this;
                cars.Add(newCar);
            }
            advance();
        }
        for (int i = 0; i < 5; i++ ) {
            try {
                cars[i].priorCar = cars[i - 1];
            }
            catch (System.ArgumentOutOfRangeException) {
            }
            try {
                cars[i].nextCar = cars[i + 1];
            }
            catch (System.ArgumentOutOfRangeException) {
            }
        }
        cars[2].getSquare().GetComponent<SquareProperties>().addState("trainStart");
        go = true;
    }

    public void advance () {
        foreach (Traincar car in cars) {
            car.advance();
            loopBreaker("train advance");
        }
        if (go == true) {
            if (currentTrack == null) {
                currentTrack = new Obstacle();
            }
            ((Midcar)cars[2]).scan();
            if (currentTrack.constituentWalls.Contains(cars[2].getSquare()) == false) {
                currentTrack.constituentWalls.Add(cars[2].getSquare());
            }
            walls.Remove(cars[2].getSquare());          
            if (cars[2].getSquare().GetComponent<SquareProperties>().isA("trainStart")
            && walls.Contains(cars[1].getSquare()) == false) {
                endTrack();
                return;
            }
        }
    }

    public GameObject mark (Vector2 wherePut, string toDisplay) {
        GameObject sign  = Instantiate(Textie, wherePut, Quaternion.identity);
        sign.transform.SetParent(canvas.transform);
        sign.GetComponent<Text>().text = toDisplay;
        return sign;
    }

    void endTrack () {
        destroyTrain();
        if (currentTrack.constituentWalls.Count >= 5) {
            Obstacle addCopy = currentTrack;
            foreach (GameObject memberSquare in addCopy.constituentWalls) {
                mark(memberSquare.GetComponent<Transform>().position, obsticalNames[0].ToString());
                memberSquare.GetComponent<SquareProperties>().greatWall = addCopy;
            }
            addCopy.nameChar = obsticalNames[0];
            obstacles.Add(addCopy);
            obsticalNames.RemoveAt(0);
        }
        go = false;
        if (walls.Count > 0) {
            currentTrack = null;
            chooChoo();
        }
    }

    void destroyTrain () {
        foreach (Traincar car in cars) {
            car.destroyCarSign();
        }
        cars.Clear();
    }

    void loopBreaker (string problemArea) {
            limiter++;
            if (limiter >= limit) {
                    throw new Exception("Infinite loop in " + problemArea + " function broken.");
            }
        }
}
