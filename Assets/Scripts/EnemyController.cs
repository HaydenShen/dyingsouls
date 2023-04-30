using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float attackDistance;
    public float moveSpeed;
    public float timer;

    public Transform leftBorder;
    public Transform rightBorder;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;

    [SerializeField] private AudioSource slayed;

    protected Animator anim;
    protected float distance;
    protected bool attackMode;

    protected bool cooling;
    protected float intTimer;

    public bool isDead;

    protected string enemyName;
    protected int health;

    Material flashMaterial;
    [SerializeField] private float duration;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    public virtual void Start()
    {
        SelectTarget();
        intTimer = timer;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = Resources.Load("Materials/Flash") as Material;
    }


    public void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && !cooling)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("attack", false);
        }
    }

    public void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(enemyName + "_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        timer = intTimer;
        attackMode = true;

        anim.SetBool("canWalk", false);
        anim.SetBool("attack", true);
    }

    public void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("attack", false);
    }


    
    public void TriggerCooling()
    {
        cooling = true;
    }

    protected bool WithinBorders()
    {
        return transform.position.x > leftBorder.position.x && transform.position.x < rightBorder.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftBorder.position);
        float distanceToRight = Vector2.Distance(transform.position, rightBorder.position);

        target = (distanceToLeft > distanceToRight) ? leftBorder : rightBorder;
        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y = (transform.position.x > target.position.x) ? 180f : 0f;
        transform.eulerAngles = rotation;
    }

    public void Die()
    {
        slayed.Play();
        cooling = false;
        attackMode = false;
        anim.SetBool("attack", false);

        anim.Play(enemyName + "_Die");
        isDead = true;
    }
    public void Revive()
    {
        anim.Play(enemyName + "_Revive");
        moveSpeed += 0.5f;
    }

    void StateManager(string stateName)
    {
        switch (stateName)
        {
            case "revived":
                isDead = false;
                break;
            default:
                break;
        }
    }

    public bool isAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(enemyName + "Attack");
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    public void TakeDamage()
    {
        Flash();
        health--;
        if (health <= 0)
        {
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        Die();
        yield return new WaitForSeconds(3);
        Revive();
        health = 10;
    }
}
