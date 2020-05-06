using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickedBehavior : MonoBehaviour {

    void OnMouseOver () {
        if (Input.GetKey(KeyCode.Mouse0)) {
            this.GetComponent<SquareProperties>().addState("wall");
        }        
    }

}
