using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStats : MonoBehaviour
{
    public enum CardType
    {
        building,
        spell,
        bonus,
        upgrade,
    }
    public int price; 
    public CardType cardType;
    [Space]
    public GameObject building;

    private void Start()
    {
        transform.GetChild(2).GetComponent<TextMesh>().text = price.ToString();
         
    }


}
