using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    public static music music1;

    [SerializeField] List<AudioClip> list;
    AudioSource audioSource;

    private void Awake()
    {
        if(music1 == null)
        {
            music1 = this;
            DontDestroyOnLoad(this);
        }
        else 
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    public void PLay(int id)
    {
        audioSource.PlayOneShot(list[id]);
    }

}
