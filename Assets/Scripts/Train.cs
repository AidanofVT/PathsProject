using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour {
    List<Traincar> cars = new List<Traincar>(5);
    public GameObject driver;
    List<GameObject> walls;

    private void Awake() {
        walls = driver.GetComponent<BeatingHeart>().wallCells;
    }

    public void chooChoo () {
        Time.fixedDeltaTime = 0.2f;
        for (int i  = 5; i > 1; i--) {
                Traincar newCar = new Traincar(walls[0].GetComponent<SquareProperties>().nameInCoordinates);
                cars.Add(newCar);
                advance();
        }
    }

    private void FixedUpdate() {
        advance();
    }

    void advance () {
        foreach (Traincar car in cars) {
            car.advance();
        }
    }
}
