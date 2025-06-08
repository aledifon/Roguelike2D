using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Sprite dmgSprite;
    [SerializeField] int hp = 3;

    [SerializeField] AudioClip chopSound1;
    [SerializeField] AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.Instance.RandomizeFx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
