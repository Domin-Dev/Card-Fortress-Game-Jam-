using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int damage;
    public float push;
    public int range;
    public bool isIce;
    public bool isFire;

    bool isReady = false;

    public void Already()
    {
        isReady = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isReady)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().Hit(damage, push,isIce,isFire);
                MapGenerator.mapGenerator.SetText(collision.transform.position, damage);
            }
        }
    }

    private void Update()
    {
        if(isReady)
        {
            Destroy(this);
        }
    }
}
