using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Animator anim;
    float speed = 4.0f;
    float jumpVelocity = 6.0f;
    enum State { idle, running, jumping, falling, shielding, dead };
    State state = State.dead;
    bool combo;
    Collider2D col;
    [SerializeField] private LayerMask ground;

    Vector3 original_position;
    bool canRevive;


    public Rigidbody2D rb;
    public SpriteRenderer rend;
    public DeathMessageController deathMessageController;

    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource attackSound1;
    [SerializeField] private AudioSource attackSound2;
    [SerializeField] private AudioSource shield;
    [SerializeField] private AudioSource throe;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0f, 0f);
        
        col = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.dead)
        {
            Move();
            SwitchState();
            anim.SetInteger("state", (int)state);
        }
        else
        {
            if (Input.anyKey && canRevive)
            {
                Revive();
                canRevive = false;
            }
        }
    }

    void Move()
    {
        float direction = Input.GetAxis("Horizontal");
        if (isAttacking())
        {
            rb.velocity = Vector3.zero;
        }

        if (state != State.shielding && !isAttacking())
        {
            if (direction < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.eulerAngles = Vector3.up * 180;
            }
            else if (direction > 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.eulerAngles = Vector3.up * 0;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        

        if (col.IsTouchingLayers(ground))
        {
            if (Input.GetKeyDown(KeyCode.W) && state != State.shielding)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                state = State.jumping;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                rb.velocity = new Vector2(0, 0);
                state = State.shielding;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                state = State.idle;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetTrigger("attack");
            }
        }
    }

    void SwitchState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < -0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (col.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            state = State.running;
        }
        else if (Mathf.Abs(rb.velocity.x) == 0f && state != State.shielding)
        {
            state = State.idle;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Attack" && state != State.dead)
        {
            if (state == State.shielding && other.gameObject.transform.parent.eulerAngles.y != transform.eulerAngles.y)
            {
                shield.Play();
            }
            else
            {
                Die();
                StartCoroutine(waiter());
            }
        }
    }

    public void Die()
    {
        throe.Play();
        deathMessageController.FadeBlack();
        anim.Play("Player_Die");
        state = State.dead;
    }

    public void Revive()
    {
        deathMessageController.FadeOut();
        anim.Play("Player_Revive");
    }

    bool isAttacking()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack1") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack3");
        
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
    }

    void AttackOffset()
    {
        if (transform.eulerAngles.y == 180)
        {
            transform.position += new Vector3(-1f, 0f, 0f);
        }
        else
        {
            transform.position += new Vector3(1f, 0f, 0f);
        }
        
    }

    void SoundManager(string actionName)
    {
        switch(actionName)
        {
            case "run":
                footstep.Play();
                break;
            case "attack":
                attackSound1.Play();
                break;
            case "final attack":
                attackSound2.Play();
                break;
            default:
                break;
        }
    }

    void StateManager(string stateName)
    {
        switch(stateName)
        {
            case "revived":
                state = State.idle;
                break;
            case "dead":
                canRevive = true;
                break;
            default:
                break;
        }
    }
}
