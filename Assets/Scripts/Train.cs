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
    bool chooed = false;

    private void Awake() {
        walls = driver.GetComponent<BeatingHeart>().wallCells;
    }

    public void chooChoo () {
        Time.fixedDeltaTime = 0.7f;
        planeCoord startPoint = walls[0].GetComponent<SquareProperties>().nameInCoordinates;
        Vector2 location = walls[0].GetComponent<Transform>().position;
        for (int i = 1; i <= 5; i++) {
            GameObject carSign = Instantiate(Textie, location, Quaternion.identity);
            carSign.transform.SetParent(canvas.transform);
            carSign.GetComponent<Text>().text = i.ToString();
            if (i == 3) {
                Midcar newCar = new Midcar(startPoint, carSign);
                cars.Add(newCar);
            }
            else {
                Traincar newCar = new Traincar(startPoint, carSign);
                cars.Add(newCar);
            }
            advance(0);
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
        chooed = true;
    }

    private void FixedUpdate() {
        advance(2);
    }

    void advance (int mode) {
        foreach (Traincar car in cars) {
            car.advance();
            loopBreaker("Train advance");
        }
        if (mode == 2 && chooed == true) {
            ((Midcar)cars[2]).scan();
        }
    }

    void loopBreaker (string problemArea) {
            limiter++;
            if (limiter >= limit) {
                    throw new Exception("Infinite loop in " + problemArea + " function broken.");
            }
        }
}
