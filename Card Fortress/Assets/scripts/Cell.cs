using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell 
{
    public GameObject building;
    public GameObject cell;
    public bool canBuild;

    List<Transform> listEnemies = new List<Transform>();

    public Cell(GameObject building, GameObject cell,bool canBuild)
    {
        if(building != null) this.building = building;
        if(cell != null) this.cell = cell;
    }

    public void SetBuilding(GameObject building)
    {
        this.building = building;
    }


}
