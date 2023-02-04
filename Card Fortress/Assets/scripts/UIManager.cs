using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;
    [SerializeField] Transform pause;
    private void Awake()
    {
        if(uIManager == null)
        {
            uIManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
            if (pause.gameObject.activeSelf)  pause.gameObject.SetActive(false);
            else pause.gameObject.SetActive(true);
            
       }
        
    }
}
