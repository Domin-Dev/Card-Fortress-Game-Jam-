using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform shotPoint;
    [SerializeField] GameObject bullet;
    Transform lifePointsBar;
    BoxCollider2D boxCollider2D;



    public int currentLifePoints;
    public int maxLifePoints;
    public int damage;
    public int range;
    public float push;

    bool canShot;
    public float interval;
    private float time;

    bool wasHit;

    private void Start()
    {    
        canShot = true;
        lifePointsBar = transform.GetChild(0);
        currentLifePoints = maxLifePoints;
        lifePointsBar.GetChild(0).GetComponent<TextMesh>().text = currentLifePoints + "/" + maxLifePoints;
        lifePointsBar.GetChild(1).transform.localScale = new Vector3((float)currentLifePoints / maxLifePoints, 1, 1);

        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(range + 0.5f - 0.05f, 2);  
    }

    private void Update()
    {
        if (nearTarget != null) target = nearTarget;
        if(target != null && target.tag != "Enemy") target = null;
        minDistance = 99999;
        nearTarget = null;


        if (time > 0)
        {
            time = time - Time.deltaTime;
        }

        if (time < 0)
        {
            canShot = true;               
        }

        if(target != null) Shot(target);


        if (wasHit)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.90f, 1, 1), Time.deltaTime * 40);
            if (transform.localScale == new Vector3(0.90f, 1, 1)) wasHit = false;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 40);
        }

    }
    private void refreshHP()
    {
        lifePointsBar.GetChild(0).GetComponent<TextMesh>().text = currentLifePoints + "/" + maxLifePoints;
        lifePointsBar.GetChild(1).transform.localScale = new Vector3((float)currentLifePoints / maxLifePoints, 1, 1);
    }

    public void Hit(int damage)
    {
        wasHit = true;
        currentLifePoints = Mathf.Clamp(currentLifePoints - damage, 0, maxLifePoints);
        refreshHP();
        if (currentLifePoints <= 0) Destroy(this.gameObject);
    }



    private void Shot(Transform target)
    {
        if(canShot)
        {
        canShot = false;
        time = interval;
        GameObject gameObject = Instantiate(bullet, shotPoint.position, Quaternion.identity, transform);

        gameObject.GetComponent<Bullet>().target = target;
        gameObject.GetComponent<Bullet>().damage = damage;
        gameObject.GetComponent<Bullet>().push = push;
        }
    }


    public Transform target;

    float minDistance;
    Transform nearTarget;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && target == null)
        {
            if(Vector2.Distance(collision.transform.position,transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(collision.transform.position, transform.position);
                nearTarget = collision.transform;
            }       
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && target == collision.transform)
        {
            target = null;
        } 
    }


   

}
