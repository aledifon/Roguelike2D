using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingCharacters
{
    [SerializeField] int playerDamage;

    private Animator anim;
    private Transform target;
    private bool skipMove;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.Instance.AddEnemyToList(this);

        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // The enemy can move?
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        // Executes the parent class method
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    // This method will be called from the GameManager
    public void MoveEnemy() 
    {
        int xDir = 0;
        int yDir = 0;

        // If the Enemy is on the same column as the player he'll move only on y dir.
        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }
    protected override void OnCantMove<T>(T component)
    {
        // Safe casting of component from T to Player Type
        // If component is not a Player type then it will returns null
        Player hitPlayer = component as Player;

        // If the casting works the Enemy will damage the Player and
        // will trigger its attack animation
        hitPlayer.LoseFood(playerDamage);
        anim.SetTrigger("EnemyAttack");
    }
}
