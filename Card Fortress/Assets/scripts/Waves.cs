using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] GameObject orc;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject slime;
    [SerializeField] GameObject spider;
    [SerializeField] Animator animatorNewWave;


    float spawnSite1;
    float spawnSite2;

    public int waveNumber;
    private void Start()
    {
        spawnSite1 = MapGenerator.mapGenerator.worldSize * 0.5f + 6; 
        spawnSite2 = - (MapGenerator.mapGenerator.worldSize * 0.5f + 6); 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            animatorNewWave.SetTrigger("new");
            for (int i = 0; i < 5; i++)
            {
                Instantiate(orc, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(orc, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);

                Instantiate(skeleton, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(skeleton, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);

                Instantiate(slime, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(slime, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);

                Instantiate(spider, new Vector3(spawnSite1, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);
                Instantiate(spider, new Vector3(spawnSite2, -Random.Range(0.8f, 1.25f), 0), Quaternion.identity);

            }


        }
    }





}
