using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private Text usedText;
    [SerializeField] private Text bagText;



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



    [SerializeField] private int[] indexCardsInHand;
    [SerializeField] private List<int> CardsInBag;
    [SerializeField] private List<int> CardsInUsed;
    [SerializeField] public List<int> cardsToUnlock;
    [SerializeField] private List<int> cardsInChoice;




    public Card selectedCard;

    int numberReplacements = 0;
    [SerializeField] Text priceText;

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
        for (int i = 0; i < cardsData.Count; i++)
        {
            if (cardsData[i].IsAtStart) CardsInBag.Add(i);
            else cardsToUnlock.Add(i);
        }
        bagText.text = "card bag" + "\n(" + CardsInBag.Count + ")";

        priceText.text = (numberReplacements * 10 + 30).ToString();
        bagAnimator = bagTransform.GetComponent<Animator>();
        usedAnimator = usedTransform.GetComponent<Animator>();
        cardsInHand = new GameObject[slotList.Count];
        indexCardsInHand = new int[slotList.Count];
        GameObject gameobj = new GameObject("used");
        gameobj.transform.parent = transform;      
        Vector3 vector3 = new Vector3(transform.position.x + 8, transform.position.y -5, 10);
        gameobj.transform.position = vector3;
        usedCards = gameobj.transform;
        ReplaceCards();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0)
        {
            BuyReplaceCards();
        }
    } 

    public void BuyReplaceCards()
    {
        if (numberReplacements * 10 + 30 <= MapGenerator.mapGenerator.money)
        {
            MapGenerator.mapGenerator.SubtractMoney(numberReplacements * 10 + 30);
            numberReplacements++;
            priceText.text = (numberReplacements * 10 + 30).ToString();
            ReplaceCards();
        }
    }




    private void ReplaceCards()
    {
        music.music1.PLay(0);
        bagAnimator.SetTrigger("new");
        MapGenerator.mapGenerator.BuildingModeOff();
        MapGenerator.mapGenerator.SpellModeOff();
        if (selectedCard != null) Destroy(selectedCard.gameObject);
        usedAnimator.SetTrigger("Added");
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
        vector3 = new Vector3(vector3.x - 2, vector3.y, 10);

        for (int i = 0; i < slotList.Count; i++)
        {
            if (null != cardsInHand[i])
            {
                obj = cardsInHand[i];
                Card card = obj.GetComponent<Card>();
                obj.GetComponent<BoxCollider2D>().enabled = false;
                card.SetStartPosition();
                card.target = usedCards;
                card.speed = 1.5f + 0.02f * i;
                obj.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                CardsInUsed.Add(indexCardsInHand[i]);
                Destroy(obj, 0.7f);
            }
        }

        vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
        vector3 = new Vector3(vector3.x - 2, vector3.y, 10);

        for (int i = 0; i < slotList.Count; i++)
        {
            int index = GetRandomCard();
            obj = MakeCard(index, vector3);
            indexCardsInHand[i] = index;
            cardsInHand[i] = obj;
            Card card = obj.GetComponent<Card>();
            card.target = slotList[i];
            card.speed = 3.5f + 0.5f * i;
            card.usedTransform = usedCards;
            card.slotIndex = i;
            obj.GetComponent<SortingGroup>().sortingOrder = slotList.Count - i;
        }


        bagText.text = "card bag" + "\n(" + CardsInBag.Count + ")";
        usedText.text = "cards used" + "\n(" + CardsInUsed.Count + ")";
    }

    public void UsedCard(int slotIndex, bool remove)
    {
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(bagTransform.transform.position);
        vector3 = new Vector3(vector3.x - 2, vector3.y, 10);
        cardsInHand[slotIndex] = null;


        if(!remove) CardsInUsed.Add(indexCardsInHand[slotIndex]);
        int index = GetRandomCard();
        obj = MakeCard(index, vector3);
        indexCardsInHand[slotIndex] = index;
        bagText.text = "card bag" + "\n(" + CardsInBag.Count + ")";
        usedText.text = "cards used" + "\n(" + CardsInUsed.Count + ")";

        cardsInHand[slotIndex] = obj;
        Card card = obj.GetComponent<Card>();
        card.target = slotList[slotIndex];
        card.speed = 3.5f + 0.5f * slotIndex;
        card.usedTransform = usedCards;
        card.slotIndex = slotIndex;
        obj.GetComponent<SortingGroup>().sortingOrder = slotList.Count - slotIndex;
        bagAnimator.SetTrigger("new");
    }

    private int GetRandomCard()
    {
        if(CardsInBag.Count == 0)
        {
            CardsInBag.AddRange(CardsInUsed);
            CardsInUsed.Clear();
            bagText.text = "card bag" + "\n(" + CardsInBag.Count + ")";
            usedText.text = "cards used" + "\n(" + CardsInUsed.Count + ")";
        }      
        int index = CardsInBag[Random.Range(0, CardsInBag.Count)];
        CardsInBag.Remove(index);
        return index;
    }

    public GameObject MakeCard(int index, Vector3 vector3)
    {
        music.music1.PLay(0);
        CardData cardData = cardsData[index];

        GameObject card = cards[0];
        switch (cardData.cardType)
        {
            case CardStats.CardType.defenceStructure:
                card = cards[0];
                break;
            case CardStats.CardType.building:
                card = cards[1];
                break;
            case CardStats.CardType.spell:
                card = cards[2];
                break;
            case CardStats.CardType.upgrade:
                card = cards[3];
                break;
        }
        
        card = Instantiate(card, vector3, Quaternion.identity, transform);
        card.SetActive(true);
        CardStats cardStats = card.GetComponent<CardStats>();
        cardStats.cardType = cardData.cardType;
        cardStats.price = cardData.price;
        cardStats.building = cardData.building;
        cardStats.spell = cardData.Spell;
        cardStats.cardName = cardData.cardName;
        cardStats.sprite = cardData.sprite;
        cardStats.description = cardData.description;
        cardStats.isTower = cardData.isTower;
        cardStats.spellID = cardData.spellID;
        cardStats.UpdateCard();

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
    public int GetIndexBySlot(int slotIndex)
    {
        return indexCardsInHand[slotIndex];
    }

    public void AddCard(int index)
    {
        cardsInChoice.Remove(index);
        cardsToUnlock.AddRange(cardsInChoice);
        cardsInChoice.Clear();
        CardsInBag.Add(index);
        bagText.text = "card bag" + "\n(" + CardsInBag.Count + ")";
        usedText.text = "cards used" + "\n(" + CardsInUsed.Count + ")";
    }




    public void SetCard(Transform transform, Text text)
    {
        if (cardsToUnlock.Count > 0)
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(transform.position);

            int index = Random.Range(0, cardsToUnlock.Count);
            cardsInChoice.Add(cardsToUnlock[index]);
            GameObject obj = MakeCard(cardsToUnlock[index], Vector3.zero);
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 5);
            obj.transform.localScale = new Vector3(400, 400, 1);
            obj.gameObject.SetActive(true);
            obj.AddComponent<CardButton>();
            obj.GetComponent<CardButton>().cardIndex = cardsToUnlock[index];
            cardsToUnlock.RemoveAt(index);

            Destroy(obj.GetComponent<BoxCollider2D>());
            obj.AddComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.96f);

            int priceIndex = Random.Range(0, 11);
            obj.GetComponent<CardButton>().priceIndex = priceIndex;

            text.text = "price:\n" + GetPriceString(priceIndex);
            Destroy(obj.GetComponent<Card>());

        }
        else
        {
            text.text = "";
        }



    }


    private string GetPriceString(int index)
    {
        switch (index)
        {
            case 0: return "increases monster damage by 5%";
            case 1: return "increases monster damage by 10%";
            case 10:return "increases monster damage by 20%";


            case 2: return "increases monsters' life points by 2%";
            case 3: return "increases monsters' life points by 5%";
            case 4: return "increases monsters' life points by 15%";

            case 5: return "increases the speed of monsters by 10%";
            case 6: return "increases the speed of monsters by 20%";


            case 7: return "reduces happiness by 10";
            case 8: return "reduces happiness by 20";
            case 9: return "reduces happiness by 40";
        }

        return "";

    }


    public void SelectedCardOff()
    {        
      if(selectedCard != null)   selectedCard.GetComponent<Card>().ComeBack();
    }

    public void SetActiveCard(bool value)
    {
        foreach (GameObject item in cardsInHand)
        {
            item.SetActive(value);
        }
    }
}
