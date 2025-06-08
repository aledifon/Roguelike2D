using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
     
    [HideInInspector] public bool playersTurn = true;

    [SerializeField] float turnDelay = 0.1f;
    [SerializeField] float levelStartDelay = 2;    

    [SerializeField] private int playerFoodPoints = 100;
    public int PlayerFoodPoints { get => playerFoodPoints; set => playerFoodPoints = value; }

    private BoardManager boardScript;    
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private int level = 0;
    private TextMeshProUGUI levelText;
    private GameObject levelImage;
    private bool doingSetup;

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

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    level++;
    //    InitGame();
    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame(); 
    }


    private void Update()
    {
        if(playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(nameof(MoveEnemies));
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.FindGameObjectWithTag("LevelImage");
        levelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextMeshProUGUI>();

        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke(nameof(HideLevelImage),levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }
    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
            yield return new WaitForSeconds(turnDelay);

        foreach (Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

    public void GameOver()
    {
        levelText.text = "Has sobrevivido a " + level + " días";
        levelImage.SetActive(true);
        enabled = false;
    }
}
