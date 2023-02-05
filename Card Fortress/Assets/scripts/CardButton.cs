using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    public int cardIndex;
    public int priceIndex;


    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Waves.waves.SetBonus(priceIndex);
            Waves.waves.SwitchOff(cardIndex);
        }
    }




}
