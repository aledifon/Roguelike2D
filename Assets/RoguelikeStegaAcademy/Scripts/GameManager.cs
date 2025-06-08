using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    //public static GameManager Instance
    //{
    //    get 
    //    {
    //        //if (instance == null)
    //        //{
    //        //    instance = FindAnyObjectByType<GameManager>();
    //        //    if (instance == null)
    //        //    {
    //        //        GameObject go = new GameObject("GameManager");
    //        //        instance = go.AddComponent<GameManager>();
    //        //    }
    //        //}
    //        return instance; 
    //    }
    //}    
    [HideInInspector] public bool playersTurn = true;
    [SerializeField] private int playerFoodPoints = 100;
    public int PlayerFoodPoints { get => playerFoodPoints; set => playerFoodPoints = value; }

    private BoardManager boardScript;

    private int level = 3;

    private void Awake()
    {        
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else        
            instance = this;                    

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        InitGame();
    }


    private void InitGame()
    {
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
