using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    bool isEnemy;

    private void Start()
    {
        isEnemy = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            isEnemy = true;
        }            
    }

    private void Update()
    {
        MapGenerator.mapGenerator.canBuild = !isEnemy;
        isEnemy = false;
    }


}
