using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CardStats : MonoBehaviour
{
    public enum CardType
    {
        building,
        spell,
        defenceStructure,
        upgrade,
    }
    public int price; 
    public string cardName;
    public string description;
    public Sprite sprite;
    public bool isTower;


    public CardType cardType;
    public GameObject building;
    public GameObject spell;
    public int spellID;

    public void UpdateCard()
    {
        if (!isTower)
        {
            transform.GetChild(3).GetComponent<TextMesh>().text = description.Replace("!", Environment.NewLine);
            if (building != null) building.GetComponent<Tower>().description = description;
        }
        else if (building != null)
        {
            Tower tower = building.GetComponent<Tower>();
            transform.GetChild(3).GetComponent<TextMesh>().text =
                "life points: " + tower.maxLifePoints.ToString() + "\n" +
                "damage: " + tower.damage.ToString() + "\n" +
                "range: " + tower.range.ToString() + "\n" +
                "Push: " + tower.push.ToString() + "\n" +
                "rate of\n fire: " + (60 / tower.interval).ToString() + "(" +tower.bonusSpeed * 100 + "%)"+ "\n";
            tower.description = description;

        }
        transform.GetChild(2).GetComponent<TextMesh>().text = price.ToString();
        transform.GetChild(1).GetComponent<TextMesh>().text = cardName;        
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;        
    }


}
