using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombThrower : MonoBehaviour
{
    [Header("Bombs config")]
    public FireBombController fireBombPrefab;
    public StoneBombController stonePrefab;

    [Tooltip("Процент коктелей молотова (остальные - камни)")]
    [Range(0, 1)]
    public float fireBombsPercent = 0.5f;
    //public float frequencyMin = 0.1f;
    [Tooltip("Частота бросков бомбы")] public float bombFrequency = 3f;
    [Tooltip("Через сколько после старта анимации происходит запуск бомбы")] public float fireBombAnimationThrow = 2.6f;
    [Tooltip("Через сколько после старта анимации происходит запуск камня")] public float rockAnimationThrow = 2f;

    bool throwingBombs;
    bool fireBomb;
    Vector2 bombPoint;
    float animationDelay;
    float nextBomb;
    float spawnBombIn;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        StartThrowingBombs();
    }

    public void StartThrowingBombs()
    {
        int layerMask = LayerMask.GetMask("buildings");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(2, 1), 10, layerMask);
        // Debug.DrawRay(transform.position, new Vector2(2, 1)*10, Color.green, 5f);

        if (hit.collider == null)
        {
            print("Target not found!");
            return;
        }
        
        bombPoint = hit.point;
        bombPoint.y += Random.Range(0.5f, 2f);

        Vector2 direction = (Vector3)bombPoint - transform.position;
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.y);

        if (Random.value < fireBombsPercent)
        {
            fireBomb = true;
            // animator.SetInteger("State", 1);
            animationDelay = fireBombAnimationThrow;
        }
        else
        {
            animationDelay = rockAnimationThrow;
        }
        
        spawnBombIn = float.PositiveInfinity;
        nextBomb = Random.Range(0, 2f);
        throwingBombs = true;
    }

    public void StopThrowingBombs()
    {
        throwingBombs = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!throwingBombs) { return; }

        CheckNextThrow();

        CheckThrownBomb();
    }

    private void CheckNextThrow()
    {
        if (nextBomb > 0)
        {
            nextBomb -= Time.deltaTime;
        }
        else
        {
            Vector2 direction = (Vector3)bombPoint - transform.position;
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
            ThrowBomb();

            nextBomb = bombFrequency + animationDelay;
        }
    }

    private void CheckThrownBomb()
    {
        if (spawnBombIn > 0)
        {
            spawnBombIn -= Time.deltaTime;
        }
        else
        {
            animator.SetInteger("State", 0);

            AbstractBombController bomb;
            if (fireBomb)
            {
                bomb = Instantiate(fireBombPrefab, transform);
                bomb.transform.localPosition = new Vector3(0, 0.8f, 0);
            }
            else
            {
                bomb = Instantiate(stonePrefab, transform);
                bomb.transform.localPosition = new Vector3(0, 0.8f, 0);
            }
            bomb.ThrowBomb(bombPoint);

            spawnBombIn = float.PositiveInfinity;
        }
    }

    private void ThrowBomb()
    {
        if (fireBomb)
        {
            animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 2);
        }
        spawnBombIn = animationDelay;
    }

}
