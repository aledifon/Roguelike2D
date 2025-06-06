using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            //if (instance == null)
            //{
            //    instance = FindAnyObjectByType<SoundManager>();
            //    if (instance == null)
            //    {
            //        GameObject go = new GameObject("SoundManager");
            //        instance = go.AddComponent<SoundManager>();
            //    }
            //}
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        DontDestroyOnLoad(gameObject);    
    }
}
