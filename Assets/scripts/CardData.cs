using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public bool IsAtStart;

    public int price;
    public string cardName;
    public string description;
    public bool isTower;

    public CardStats.CardType cardType;
    public Sprite sprite;

    [Space(30)]public GameObject building;
    [Space(30)]
    public GameObject Spell;
    public int spellID;



}