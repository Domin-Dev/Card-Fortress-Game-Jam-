using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    Transform kingTransform;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Vector2 directionMovement;
    [SerializeField] Sprite sprite;
    public int currentLifePoints;
    public int maxLifePoints;
    public int damage;
    public float range;
    public float speed;
    [Space]
    public int cellID;

    bool wasHit = false;
    bool isIced = false;
    bool burns = false;

    public float speedBonus2;
    public float hpBonus;
    public float damageBonus;


    private float maxPositionX = 9999;

    float speedBonus = 1f;


    private void Start()
    {
        speedBonus2 = Waves.waves.speedBonus;
        hpBonus = Waves.waves.hpBonus;
        damageBonus = Waves.waves.damageBonus;


        speedBonus = 1f;
        currentLifePoints = maxLifePoints  + Mathf.RoundToInt(maxLifePoints * hpBonus);


        animator = GetComponent<Animator>();
    
        spriteRenderer = GetComponent<SpriteRenderer>();
        kingTransform = GameObject.FindGameObjectWithTag("KingTower").transform;
        if(kingTransform.position.x + transform.position.x > 0)
        {
            directionMovement = Vector2.left;
            spriteRenderer.flipX = true;        
        }
        else
        {
            directionMovement = Vector2.right;
            spriteRenderer.flipX = false;
        }
    }

    float stoper;

    private void SetHitEffect()
    {
        wasHit = true;
        stoper = 0.1f;
        spriteRenderer.material = MapGenerator.mapGenerator.white;
    }
    private void Update()
    {
        if(stoper > 0)
        {
            stoper = stoper - Time.deltaTime;
        }

        if(stoper <= 0)
        {
            wasHit = false;
            spriteRenderer.material = MapGenerator.mapGenerator.normal;
        }


        if (maxPositionX == 9999)
        {
            animator.SetBool("attack", false);
            transform.position = transform.position + (Vector3)directionMovement *(speed + (speed * speedBonus2) ) * speedBonus * Time.deltaTime;
        }
        else
        {

            if(directionMovement.x == 1 && transform.position.x <= maxPositionX
                || directionMovement.x == -1 && transform.position.x >= maxPositionX)
            {
                animator.SetBool("attack", false);
                transform.position = transform.position + (Vector3)directionMovement * (speed + (speed * speedBonus2)) * speedBonus * Time.deltaTime;
            }
            else
            {
                animator.SetBool("attack",true);
            }
        }

        if(cellID != Mathf.RoundToInt(transform.position.x / 0.5f))
        {
            cellID = Mathf.RoundToInt(transform.position.x / 0.5f);
            
            if (MapGenerator.mapGenerator.CheckBuilding(cellID + (int)directionMovement.x))
            {
                maxPositionX = cellID * 0.5f;
            }
            else
            {
                maxPositionX = 9999;
            }
        }



        if(wasHit)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.3f, 1.5f, 1), Time.deltaTime * 10);
            if (transform.localScale == new Vector3(0.3f, 2, 1)) wasHit = false;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
        }

        if(isIced)
        {
            iceStoper = iceStoper - Time.deltaTime;
            if(iceStoper <= 0)
            {
                isIced = false;
                spriteRenderer.color = Color.white;
                speedBonus = 1f;
            }
        }
        
        if(burns)
        {
            burnsStoper = burnsStoper - Time.deltaTime;

            time = time - Time.deltaTime;
            if(time <= 0)
            {
                time = 1f;
                Hit(2, 0, false, false);
                MapGenerator.mapGenerator.SetText(transform.position, 2);
            }

            if(burnsStoper <= 0)
            {
                burns = false;
                spriteRenderer.color = Color.white;
            }
        }


    }
    float iceStoper;
    float burnsStoper;
    float time;
    public void attackBuilding()
    {
        MapGenerator.mapGenerator.Hit(damage + Mathf.RoundToInt(damage * damageBonus), cellID + (int)directionMovement.x);
        cellID = Mathf.RoundToInt(transform.position.x / 0.5f);
        animator.SetBool("attack", false);
        if (MapGenerator.mapGenerator.CheckBuilding(cellID + (int)directionMovement.x))
        {
            maxPositionX = cellID * 0.5f;
        }
        else
        { 
            maxPositionX = 9999;
        }
    }

    private void Ice()
    {
        burnsStoper = 0;
        isIced = true;
        spriteRenderer.color = Color.cyan;
        speedBonus = 0.3f;
        iceStoper = 10f;
    }
    
    private void Fire()
    {
        if (!burns) time = 1;
        iceStoper = 0;
        burns = true;
        spriteRenderer.color = Color.red;
        burnsStoper = 10f;
    }

    public void Hit(int damage,float push,bool ice,bool fire)
    {
        if (ice) Ice();
        if (fire) Fire();
        wasHit = true;
        SetHitEffect();
        transform.position = transform.position - new Vector3(0.01f * push * directionMovement.x, 0, 0);
        currentLifePoints = Mathf.Clamp(currentLifePoints - damage, 0, maxLifePoints + Mathf.RoundToInt(maxLifePoints * hpBonus));
        if (currentLifePoints <= 0)
        {
            Instantiate(MapGenerator.mapGenerator.bloodEffect, transform.position, Quaternion.identity);
            spriteRenderer.material = MapGenerator.mapGenerator.normal;
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = -3;
            spriteRenderer.color = Color.gray;
            animator.enabled = false;
            gameObject.tag = "Body";
            speed = 0;
            this.damage = 0;
            this.enabled = false;
            Waves.waves.MonsterWasKilled();
            Destroy(gameObject,10);
        }
    }


    
}

