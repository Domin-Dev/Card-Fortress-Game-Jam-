using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardsManager : MonoBehaviour
{
    public static CardsManager cardsManager;
    [SerializeField] private List<Transform> slotList;
    [SerializeField] private GameObject[] cardsInHand;
  
    

    [SerializeField] Transform  bagTransform;
    Animator bagAnimator;
    [SerializeField] Transform  usedTransform;
    Animator usedAnimator;
    [SerializeField] Animator coinAnimator;


    public Transform usedCards;
    [SerializeField] private List<GameObject> cards;
    [SerializeField] private List<CardData> cardsData;

    public Card selectedCard;

    private void Awake()
    {
        if(cardsManager == null)
        {
            cardsManager = this;
        }
        else
        {
            Destroy(this);
        }
    }

    GameObject obj;
    private void Start()
    {
        bagAnimator = bagTransform.GetComponent<Animator>();
        usedAnimator = usedTransform.GetComponent<Animator>();
        cardsInHand = new GameObject[slotList.Count];
        GameObject gameobj = new GameObject("used");
        gameobj.transform.parent = transform;
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(usedTransform.transform.position);
        vector3 = new Vector3(vector3.x + 2, vector3.y -2, 10);
        gameobj.transform.position = vector3;
        usedCards = gameobj.transform;
    }

    bool isOnSite = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bagAnimator.SetTrigger("new");
            MapGenerator.mapGenerator.BuildingModeOff();
            if (selectedCard != null) Destroy(selectedCard.gameObject);
            usedAnimator.SetTrigger("Added");
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
            vector3 = new Vector3(vector3.x - 2, vector3.y, 10);

            for (int i = 0; i < slotList.Count; i++)
            {
                if(null != cardsInHand[i])
                {
                    obj = cardsInHand[i];
                    Card card = obj.GetComponent<Card>();
                    obj.GetComponent<BoxCollider2D>().enabled = false;
                    card.SetStartPosition();
                    card.target = usedCards;
                    card.speed = 1.5f + 0.02f * i;
                    obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    Destroy(obj, 0.7f);
                }
            }   

            vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
            vector3 = new Vector3(vector3.x - 2, vector3.y, 10);

            for (int i = 0; i < slotList.Count; i++)
            {
                obj = MakeCard(cardsData[Random.Range(0, cardsData.Count)], vector3);
                cardsInHand[i] = obj;
                Card card = obj.GetComponent<Card>();
                card.target = slotList[i];
                card.speed = 3.5f + 0.5f * i;
                card.usedTransform = usedCards;
                card.slotIndex = i;
                obj.GetComponent<SortingGroup>().sortingOrder = slotList.Count - i;
            }


        }
    } 

    public void UsedCard(int slotIndex)
    {
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
        vector3 = new Vector3(vector3.x - 2, vector3.y, 10);
        cardsInHand[slotIndex] = null;


        obj = MakeCard(cardsData[Random.Range(0, cardsData.Count)], vector3);
        cardsInHand[slotIndex] = obj;
        Card card = obj.GetComponent<Card>();
        card.target = slotList[slotIndex];
        card.speed = 3.5f + 0.5f * slotIndex;
        card.usedTransform = usedCards;
        card.slotIndex = slotIndex;
        obj.GetComponent<SortingGroup>().sortingOrder = slotList.Count - slotIndex;
        bagAnimator.SetTrigger("new");
    }

    public GameObject MakeCard(CardData cardData, Vector3 vector3)
    {
        GameObject card = cards[0];
        switch (cardData.cardType)
        {
            case CardStats.CardType.building:
                card = cards[0];
                break;
            case CardStats.CardType.spell:
                card = cards[1];
                break;
            case CardStats.CardType.bonus:
                card = cards[2];
                break;
            case CardStats.CardType.upgrade:
                card = cards[3];
                break;
        }
        card = Instantiate(card, vector3, Quaternion.identity, transform);

        return card;
    }


    public void StartAnimation()
    {
        usedAnimator.SetTrigger("Added");
    }

    private void OnMouseEnter()
    {
        transform.position = transform.position - new Vector3(0.5f, 0, 0);
    }

    private void OnMouseExit()
    {
        transform.position = transform.position + new Vector3(0.5f, 0, 0);
    }

    public void Enough(bool value)
    {
        coinAnimator.SetBool("enough", value);
    }
}
