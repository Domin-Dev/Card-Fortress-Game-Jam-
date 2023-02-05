using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;
    [SerializeField] Transform endWave;
    [SerializeField] Transform pause;

    private void Awake()
    {

        if(uIManager == null)
        {
            uIManager = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        FindObjectOfType<Waves>().SetTime1x();
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape) && !endWave.gameObject.activeSelf)
       {
            if (pause.gameObject.activeSelf)
            {
                FindObjectOfType<Waves>().SetTime1x();
                pause.gameObject.SetActive(false);
                CardsManager.cardsManager.SetActiveCard(true);
            }
            else
            {
                CardsManager.cardsManager.SelectedCardOff();                 
                Time.timeScale = 0f;
                pause.gameObject.SetActive(true);
                CardsManager.cardsManager.SetActiveCard(false);
                if (FindObjectsOfType<Card>().Length > 0)
                {
                    foreach (Card item in FindObjectsOfType<Card>())
                    {
                       item.gameObject.SetActive(false);
                    }
                }
                    
            }
            
       }
      
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        FindObjectOfType<Waves>().SetTime1x();
        pause.gameObject.SetActive(false);
        CardsManager.cardsManager.SetActiveCard(true);
    }
}
