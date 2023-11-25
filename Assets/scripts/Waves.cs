using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waves : MonoBehaviour
{
    public static Waves waves;
    [SerializeField] Transform windowChoice;


    public float speedBonus = 0;
    public float hpBonus = 0;
    public float damageBonus = 0;





    [SerializeField] Sprite button1;
    [SerializeField] Sprite button2;

    [SerializeField] Image time2x;
    [SerializeField] Image time1x;



    [SerializeField] GameObject orc;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject slime;
    [SerializeField] GameObject spider;
    [SerializeField] Animator animatorNewWave;


    [SerializeField] Text waveText;

    [SerializeField] Image timerBar;
    [SerializeField] Text timerText;

    float timer;
   const float timeWave = 10;

    float spawnSite1;
    float spawnSite2;

     int waveNumber;
     int monsterNumber;
    private void Awake()
    {
        if(waves == null)
        {
            waves = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        monsterNumber = 0;
        waveNumber = 0;
        RefreshText();
        timer = timeWave;
        spawnSite1 = MapGenerator.mapGenerator.worldSize * 0.5f + 6; 
        spawnSite2 = - (MapGenerator.mapGenerator.worldSize * 0.5f + 6); 
    }

    private void Update()
    {
        if (timer > 0)
        {
            end = true;
            timer = timer - Time.deltaTime;
            timerBar.fillAmount = 1 - timer / timeWave;
            timerText.text = "next wave in: " + Mathf.Round(timer).ToString();
        }
        else if(monsterNumber <= 0)
        {
            NextWave();
        }


/*
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CardsManager.cardsManager.cardsToUnlock.Count > 0)
            {
                windowChoice.gameObject.SetActive(true);
                Time.timeScale = 0f;

                if (MapGenerator.mapGenerator.rangeObj != null) Destroy(MapGenerator.mapGenerator.rangeObj);
                CardsManager.cardsManager.SelectedCardOff();
                CardsManager.cardsManager.SetActiveCard(false);
                if (FindObjectsOfType<Card>().Length > 0)
                {
                    foreach (Card item in FindObjectsOfType<Card>())
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                foreach (Tower item in FindObjectsOfType<Tower>())
                {
                    item.GetComponent<BoxCollider2D>().enabled = false;
                }

                foreach (Spell item in FindObjectsOfType<Spell>())
                {
                    item.GetComponent<BoxCollider2D>().enabled = false;
                }


                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(1), windowChoice.transform.GetChild(1).GetChild(0).GetComponent<Text>());
                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(2),windowChoice.transform.GetChild(2).GetChild(0).GetComponent<Text>());
                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(3), windowChoice.transform.GetChild(3).GetChild(0).GetComponent<Text>());
            }
        }

        */
        

    }

    public void SwitchOff(int index)
    {
        CardsManager.cardsManager.AddCard(index);

       if (windowChoice.transform.GetChild(1).childCount > 1) Destroy(windowChoice.transform.GetChild(1).GetChild(1).gameObject);
       if (windowChoice.transform.GetChild(2).childCount > 1) Destroy(windowChoice.transform.GetChild(2).GetChild(1).gameObject);
       if (windowChoice.transform.GetChild(3).childCount > 1) Destroy(windowChoice.transform.GetChild(3).GetChild(1).gameObject);

        windowChoice.gameObject.SetActive(false);
        SetTime1x();
        CardsManager.cardsManager.SetActiveCard(true);
        foreach (Tower item in FindObjectsOfType<Tower>())
        {
            item.GetComponent<BoxCollider2D>().enabled = true;
        }

        foreach (Spell item in FindObjectsOfType<Spell>())
        {
            item.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void NextWave()
    {
        timerText.text = " ";
        waveNumber++;
        monsterNumber = 0;
        animatorNewWave.SetTrigger("new");

        for (int i = 0; i < Mathf.RoundToInt(waveNumber *  1.5f); i++)
        {
            monsterNumber = monsterNumber + 2;
            Instantiate(slime, new Vector3(spawnSite1 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity) ;
            Instantiate(slime, new Vector3(spawnSite2 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        }

        if(waveNumber > 3 )
        {
            for (int i = 0; i < Mathf.RoundToInt(waveNumber * 1.2f); i++)
            {
                monsterNumber = monsterNumber + 2;
                Instantiate(spider, new Vector3(spawnSite1 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(spider, new Vector3(spawnSite2 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
            }
        } 
        
        if(waveNumber > 10)
        {
            for (int i = 0; i < Mathf.RoundToInt(waveNumber * 1f) - 7; i++)
            {
                monsterNumber = monsterNumber + 2;
                Instantiate(skeleton, new Vector3(spawnSite1 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(skeleton, new Vector3(spawnSite2 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
            }
        }
        
        if(waveNumber > 15)
        {
            for (int i = 0; i < Mathf.RoundToInt(waveNumber * 0.5f) - 12; i++)
            {
                monsterNumber = monsterNumber + 2;
                Instantiate(orc, new Vector3(spawnSite1 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(orc, new Vector3(spawnSite2 + Random.Range(-0.99f, 1), -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
            }
        }


        //     Instantiate(orc, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        //    Instantiate(orc, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        //    Instantiate(skeleton, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        //    Instantiate(skeleton, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        //    Instantiate(slime, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        //    Instantiate(slime, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        RefreshText();
    }

    bool end;
    public void MonsterWasKilled()
    {
        monsterNumber = Mathf.Clamp(monsterNumber - 1, 0, int.MaxValue);
        if (monsterNumber <= 0 && end)
        {
            end = false;
            if (CardsManager.cardsManager.cardsToUnlock.Count > 0)
            {
                windowChoice.gameObject.SetActive(true);
                Time.timeScale = 0f;

                if (MapGenerator.mapGenerator.rangeObj != null) Destroy(MapGenerator.mapGenerator.rangeObj);
                CardsManager.cardsManager.SelectedCardOff();
                CardsManager.cardsManager.SetActiveCard(false);
                if (FindObjectsOfType<Card>().Length > 0)
                {
                    foreach (Card item in FindObjectsOfType<Card>())
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                foreach (Tower item in FindObjectsOfType<Tower>())
                {
                    item.GetComponent<BoxCollider2D>().enabled = false;
                }

                foreach (Spell item in FindObjectsOfType<Spell>())
                {
                    item.GetComponent<BoxCollider2D>().enabled = false;
                }


                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(1), windowChoice.transform.GetChild(1).GetChild(0).GetComponent<Text>());
                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(2), windowChoice.transform.GetChild(2).GetChild(0).GetComponent<Text>());
                CardsManager.cardsManager.SetCard(windowChoice.transform.GetChild(3), windowChoice.transform.GetChild(3).GetChild(0).GetComponent<Text>());
            }


            timer = timeWave;
        }
        RefreshText();
    }

    private void RefreshText()
    {
        waveText.text = "wave: " + waveNumber + "\nmonsters: " + monsterNumber;
    }

    public void SetTime2x()
    {
        FindObjectOfType<CameraMovement>().cameraSpeed = 8f;
        time1x.sprite = button1;
        time2x.sprite = button2;
        Time.timeScale = 2f;

    }
    
    public void SetTime1x()
    {
        FindObjectOfType<CameraMovement>().cameraSpeed = 16f;
        time1x.sprite = button2;
        time2x.sprite = button1;
        Time.timeScale = 1f;
    }
    
    public void SetTime0x()
    {
        time1x.sprite = button1;
        time2x.sprite = button1;
        Time.timeScale = 0f;
    }

    public void SetBonus(int index)
    {
        switch (index)
        {
            case 0: damageBonus += 0.05f; break;
            case 1: damageBonus += 0.10f; break;
            case 10: damageBonus += 0.20f; break;

            case 2: hpBonus += 0.02f; break;
            case 3: hpBonus += 0.05f; break;
            case 4: hpBonus += 0.15f; break;

            case 5: speedBonus += 0.10f; break;
            case 6: speedBonus += 0.20f; break;

            case 7: MapGenerator.mapGenerator.AddHappiness(-10); break;
            case 8: MapGenerator.mapGenerator.AddHappiness(-20); break;
            case 9: MapGenerator.mapGenerator.AddHappiness(-40); break;

        }
    }


}
