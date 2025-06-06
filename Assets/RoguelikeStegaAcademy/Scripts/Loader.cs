using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    // I'll add the GameManager Prefab from the Inspector
    [SerializeField] private GameObject gameManager;

    [SerializeField] private GameObject soundManager;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }        
        
        if (SoundManager.Instance == null)
        {
            Instantiate(soundManager);
        }        

    }

}
