using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour {
    List<Traincar> cars = new List<Traincar>(5);
    public GameObject driver;
    List<GameObject> walls;
    public Canvas canvas;
    public GameObject Textie;
    int limit = 500;
    int limiter = 0;
    bool go = false;

    private void Awake() {
        walls = driver.GetComponent<BeatingHeart>().wallCells;
    }

    public void conduct () {
        chooChoo();
        while (walls.Count > 0) {
            advance();
            loopBreaker("Conduct");
        }
        Debug.Log("Finished crawling.");
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
        cars[2].getSquare().GetComponent<SquareProperties>().changeState("trainStart");
        go = true;
    }

    // private void FixedUpdate() {
    //     advance();
    // }

    public void advance () {
        foreach (Traincar car in cars) {
            car.advance();
            loopBreaker("train advance");
        }
        if (go == true) {
            ((Midcar)cars[2]).scan();
            walls.Remove(cars[2].getSquare());
            if (walls.Count == 0) {
                return;
            }
            else if (cars[2].getSquare().GetComponent<SquareProperties>().getState() == "trainStart"
                    && walls.Contains(cars[1].getSquare()) == false) {
                foreach (Traincar car in cars) {
                    car.destroyCarSign();
                }
                cars.Clear();
                go = false;
                chooChoo();
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

    void loopBreaker (string problemArea) {
            limiter++;
            if (limiter >= limit) {
                    throw new Exception("Infinite loop in " + problemArea + " function broken.");
            }
        }
}
