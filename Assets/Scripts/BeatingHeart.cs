using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatingHeart : MonoBehaviour {
    public int sideLength = 10;
    public planeCoord heartStart;
    public planeCoord heartFinish;
    public GameObject[,] grid;
    public List<GameObject> wallCells = new List<GameObject>();
    public Hashtable identifiedObstacles = new Hashtable();
    ClickedCellContainer clickedCellContainer;
    GenerateGrid gridBuilder;
    public GameObject pathfinder;

    void Start() {
        heartStart = new planeCoord(0,0);
        heartFinish = new planeCoord(sideLength - 1, sideLength - 1);
        gridBuilder = gameObject.GetComponent<GenerateGrid>();
        grid = gridBuilder.generateGrid(sideLength);
        clickedCellContainer = gameObject.GetComponent<ClickedCellContainer>();
        pathfinder.GetComponent<CornerPathfinder>().pathfinderGrid = grid;
        pathfinder.GetComponent<CornerPathfinder>().startUp(heartStart, heartFinish);
    }

    void Update() {
        bool mouseUpThisFrame = Input.GetKeyUp(KeyCode.Mouse0);
        if (mouseUpThisFrame) {
            List<planeCoord> toChange = clickedCellContainer.get();
            foreach (planeCoord entry in toChange) {
                grid[entry.x, entry.y].GetComponent<SquareProperties>().clickedThisMouseDown = false;
                if (wallCells.Contains(grid[entry.x, entry.y]) == false) {
                    wallCells.Add(grid[entry.x, entry.y]);
                }
                else {
                    wallCells.Remove(grid[entry.x, entry.y]);
                }
            }
            clickedCellContainer.clear();
        }        
    }
}
