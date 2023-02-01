using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    SortingGroup sortingGroup;
    int sortingOrder;
    public Vector3 startPosition;
    bool comeBack;
    Animator animator;
    public Transform target;
    public Transform usedTransform;
    CardStats cardStats;
    public int slotIndex;

    private void Start()
    {
        cardStats = GetComponent<CardStats>();
        GetComponent<BoxCollider2D>().enabled = false;
        animator = GetComponent<Animator>();
        sortingGroup = GetComponent<SortingGroup>();
        sortingOrder = sortingGroup.sortingOrder;
        startPosition = transform.localPosition;
    }
    public float speed = 10;

    bool isnUsed;
    bool isUsed;
    private void Update()
    {

        if (isUsed)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, usedTransform.localPosition, 4 * Time.deltaTime);
            if(Vector2.Distance(transform.localPosition, usedTransform.localPosition) <= 1.5f && Vector2.Distance(transform.localPosition, usedTransform.localPosition) > 1.36f)
            {
                CardsManager.cardsManager.StartAnimation();
            }
        }
        else
        {
            if (isnUsed)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, 9 * Time.deltaTime);
                if (Vector2.Distance(transform.localPosition, startPosition) <= 0.01f)
                {
                    isnUsed = false;
                    GetComponent<BoxCollider2D>().enabled = true;
                }
            }

            if (target != null)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                if (Vector2.Distance(transform.position, target.position) <= 0.2f)
                {
                    startPosition = transform.localPosition;
                    GetComponent<BoxCollider2D>().enabled = true;
                    target = null;
                }
            }
            else
            {
                if (transform.localPosition == startPosition && transform.localScale == new Vector3(1.3f, 1.3f, 1.3f))
                {
                    comeBack = false;
                }

                if (comeBack)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, 9 * Time.deltaTime);
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.3f, 1.3f, 1.3f), 7 * Time.deltaTime);

                }
            }
        }
    }

    public void SetStartPosition()
    {
        transform.localPosition = startPosition;
    }

    private void OnMouseDrag()
    {
        if(cardStats.price <=  MapGenerator.mapGenerator.money)
        {
            CardsManager.cardsManager.selectedCard = this;
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (transform.position.y > -2f)
            {
                if (cardStats.cardType == CardStats.CardType.building) MapGenerator.mapGenerator.BuildingMode(vector3.x, cardStats.building);
            }
            else
            {
                if (cardStats.cardType == CardStats.CardType.building) MapGenerator.mapGenerator.BuildingModeOff();
            }



            transform.position = new Vector3(vector3.x, vector3.y, 10);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.5f, 1), 6 * Time.deltaTime);
        }
    }

    private void OnMouseUp()
    {
        if (transform.position.y > -2f)
        {
            if (cardStats.cardType == CardStats.CardType.building) if (MapGenerator.mapGenerator.Build())
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    MapGenerator.mapGenerator.SubtractMoney(5);
                    isUsed = true;
                    CardsManager.cardsManager.UsedCard(slotIndex);
                    Destroy(gameObject,2f);
                }
        }
        else
        {
            if (cardStats.cardType == CardStats.CardType.building) MapGenerator.mapGenerator.BuildingModeOff();
        }

        if (CardsManager.cardsManager.selectedCard == this)
        {
            isnUsed = true;
            CardsManager.cardsManager.selectedCard = null;
            sortingGroup.sortingOrder = sortingOrder;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            animator.SetBool("selected", false);
        }

    }

    private void OnMouseExit()
    {
        if(CardsManager.cardsManager.selectedCard == null)
        {
            sortingGroup.sortingOrder = sortingOrder;
            comeBack = true;
            animator.SetBool("selected", false);
            CardsManager.cardsManager.Enough(true);
        }
    }

    private void OnMouseOver()
    {
        if(CardsManager.cardsManager.selectedCard == null)
        {
            if (cardStats.price <= MapGenerator.mapGenerator.money) CardsManager.cardsManager.Enough(true);
            else CardsManager.cardsManager.Enough(false);
            comeBack = false;
            animator.SetBool("selected", true);
            sortingGroup.sortingOrder = 10;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition + new Vector3(0, 0.55f, 0),5 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2,2,2), 4 * Time.deltaTime) ;
        }
    }

   
}
