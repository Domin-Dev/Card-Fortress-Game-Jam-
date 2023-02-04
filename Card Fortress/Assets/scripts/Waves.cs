using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waves : MonoBehaviour
{
    public static Waves waves;

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
   const float timeWave = 15;

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
            timer = timer - Time.deltaTime;
            timerBar.fillAmount = 1 - timer / timeWave;
            timerText.text = "next wave in: " + Mathf.Round(timer).ToString();
        }
        else if(monsterNumber <= 0)
        {
            NextWave();
        }



        if (Input.GetKeyDown(KeyCode.C))
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        timerText.text = " ";
        waveNumber++;
        monsterNumber = 8;
        RefreshText();
        animatorNewWave.SetTrigger("new");
        Instantiate(orc, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(orc, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(skeleton, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(skeleton, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(slime, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(slime, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(spider, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
        Instantiate(spider, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
    }

    public void MonsterWasKilled()
    {
        monsterNumber = Mathf.Clamp(monsterNumber - 1, 0, int.MaxValue);
        if (monsterNumber <= 0)
        {
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

}
