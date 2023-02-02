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

    

    private float maxPositionX = 9999;

    private void Start()
    {
        currentLifePoints = maxLifePoints;

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
            transform.position = transform.position + (Vector3)directionMovement * speed * Time.deltaTime;
        }
        else
        {

            if(directionMovement.x == 1 && transform.position.x <= maxPositionX
                || directionMovement.x == -1 && transform.position.x >= maxPositionX)
            {
                animator.SetBool("attack", false);
                transform.position = transform.position + (Vector3)directionMovement * speed * Time.deltaTime;
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

    }

    public void attackBuilding()
    {
        MapGenerator.mapGenerator.Hit(damage, cellID + (int)directionMovement.x);
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

    public void Hit(int damage,float push)
    {
        wasHit = true;
        SetHitEffect();
        transform.position = transform.position - new Vector3(0.01f * push * directionMovement.x, 0, 0);
        currentLifePoints = Mathf.Clamp(currentLifePoints - damage, 0, maxLifePoints);
        if (currentLifePoints <= 0)
        {
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

