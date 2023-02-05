using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform shotPoint;
    [SerializeField] GameObject bullet;
    Transform lifePointsBar;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    public bool isTower = true;
    public bool isAristocratic = false;

    public string towerName;
    public string description;
    public int currentLifePoints;
    public int maxLifePoints;
    public int damage;
    public int range;
    public float push;
    public int income = 0;
    public int happiness = 0;

    bool canShot;
    public float interval;
    public float bonusSpeed =  0;
    private float time;

    bool wasHit;
    bool shot;
    private void Start()
    {
        shot = true;  
        canShot = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifePointsBar = transform.GetChild(0);
        currentLifePoints = maxLifePoints;
        lifePointsBar.GetChild(0).GetComponent<TextMesh>().text = currentLifePoints + "/" + maxLifePoints;
        lifePointsBar.GetChild(1).transform.localScale = new Vector3((float)currentLifePoints / maxLifePoints, 1, 1);

        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(range + 0.5f - 0.05f, 2);
        if (income > 0)
        { 
            if(isAristocratic && Mathf.Abs(transform.position.x) == 0.5f)
            {
                income = 15;
            }
            MapGenerator.mapGenerator.AddIncome(income);
        }
        MapGenerator.mapGenerator.AddHappiness(happiness);


    }
    float stoper;
    private void Update()
    {
        if (isTower)
        {
            if (nearTarget != null) target = nearTarget;
            if (target != null && target.tag != "Enemy") target = null;
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

            if (target != null) Shot(target);

        }

        if (shot)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.8f, 1.05f, 1), Time.deltaTime * 30);
            if (transform.localScale == new Vector3(0.8f, 1.05f, 1)) shot = false;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 30);
        }


        if (stoper > 0)
        {
            stoper = stoper - Time.deltaTime;
        }

        if (stoper <= 0)
        {
            spriteRenderer.material = MapGenerator.mapGenerator.normal;
        }



    }



    private void SetHitEffect()
    {
        stoper = 0.08f;
        spriteRenderer.material = MapGenerator.mapGenerator.white;
    }

    private void refreshHP()
    {
        lifePointsBar.GetChild(0).GetComponent<TextMesh>().text = currentLifePoints + "/" + maxLifePoints;
        lifePointsBar.GetChild(1).transform.localScale = new Vector3((float)currentLifePoints / maxLifePoints, 1, 1);
    }

    bool destroyed = false;
    public void Hit(int damage)
    {
        SetHitEffect();
        currentLifePoints = Mathf.Clamp(currentLifePoints - damage, 0, maxLifePoints);
        refreshHP();
        if (currentLifePoints <= 0 && !destroyed)
        {
            destroyed = true;
            MapGenerator.mapGenerator.AddHappiness(-1);
            if (income > 0) MapGenerator.mapGenerator.SubtractIncome(income);

            if (gameObject.tag == "KingTower") MapGenerator.mapGenerator.End();
            Instantiate(MapGenerator.mapGenerator.destructionEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }

    public void Heal(float bonusHP)
    {
        currentLifePoints = Mathf.Clamp(currentLifePoints + Mathf.RoundToInt(bonusHP * maxLifePoints), 0, maxLifePoints);
        refreshHP();
    }


    private void Shot(Transform target)
    {
        if(canShot)
        {
            shot = true;
        canShot = false;
        time = interval - (interval * bonusSpeed);
        GameObject gameObject = Instantiate(bullet, shotPoint.position, Quaternion.identity);

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
