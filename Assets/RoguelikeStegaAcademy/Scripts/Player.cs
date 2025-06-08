using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.UI;
using Unity.VisualScripting;

public class Player : MovingCharacters
{
    [SerializeField] float restartLevelDelay = 1f;
    [SerializeField] int pointsPerFood = 10;
    [SerializeField] int pointsPerSoda = 20;
    [SerializeField] int wallDamage = 1;

    [SerializeField] AudioClip moveSound1;
    [SerializeField] AudioClip moveSound2;
    [SerializeField] AudioClip eatSound1;
    [SerializeField] AudioClip eatSound2;
    [SerializeField] AudioClip drinkSound1;
    [SerializeField] AudioClip drinkSound2;
    [SerializeField] AudioClip gameOverSound;

    private TextMeshProUGUI foodText;

    private Animator anim;
    private int food;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        food = GameManager.Instance.PlayerFoodPoints;
        foodText = GameObject.FindGameObjectWithTag("FoodText").
            GetComponent<TextMeshProUGUI>();
        foodText.text = "Food: " + food;

        base.Start();
    }
    
    private void OnDisable()
    {
        Debug.Log($"[OnDisable] Ejecutado en: {gameObject.name}");
        GameManager.Instance.PlayerFoodPoints = food;
    }

    void Update()
    {
        if(!GameManager.Instance.playersTurn) return;

        // We'll use GetAxisRaw instead of GetAxis as it will returns -1, 0 or 1 values
        // instead of returning intermediate values between -1 and 1.
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // Assure the player won't move on diagonal direction
        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // Decrease his food counter & show it in the UI
        food--;
        foodText.text = "Food: " + food;

        // Executes the parent class method
        base.AttemptMove<T>(xDir, yDir);

        //
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomizeFx(moveSound1, moveSound2);
        }
        //

        CheckIfGameOver();
        GameManager.Instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        // Safe casting of component from T to Wall Type
        // If component is not a Wall type then it will returns null
        Wall hitWall = component as Wall;

        // If the casting works the player will damage the wall and
        // will trigger the attack animation
        hitWall.DamageWall(wallDamage);
        anim.SetTrigger("Attack");
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // This Method is called every time the Enemy attacks the player
    public void LoseFood(int loss)
    {
        anim.SetTrigger("Hit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    private void CheckIfGameOver()
    {
        if(food <= 0)
        {
            SoundManager.Instance.PlaySingle(gameOverSound);
            SoundManager.Instance.StopMusic();
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
        {
            Invoke(nameof(Restart), restartLevelDelay);
            enabled = false;            
        }
        else if (collision.CompareTag("Food"))
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.Instance.RandomizeFx(eatSound1, eatSound2);
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.Instance.RandomizeFx(drinkSound1, drinkSound2);
            collision.gameObject.SetActive(false);
        }
    }
}
