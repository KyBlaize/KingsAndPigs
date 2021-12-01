using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2D;

    public float speed;
    public float jumpForce;
    public float checkRadius;
    public float attackDelayStartTime;
    public float attackBoxX, attackBoxY;
    public Transform groundCheck, attackPos;
    public LayerMask whatIsGround, damageable;

    private float horiz = 0;
    private bool isGrounded;
    private float attackDelay;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Ground Control
        horiz = Input.GetAxisRaw("Horizontal");
        if (horiz != 0)
        {
            animator.SetBool("Moving", true);
            if (horiz > 0)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            else if (horiz < 0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody2D.velocity = Vector2.up * jumpForce;
        }

        if (isGrounded)
            animator.SetBool("Falling", false);
        if (rigidbody2D.velocity.y < 0)
            animator.SetBool("Falling", true);

        //attack
        if(attackDelay <= 0)
        {
            if (Input.GetButton("Fire1"))
            {
                Collider2D[] entitiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackBoxX, attackBoxY), 0, damageable);
                for (int i = 0; i < entitiesToDamage.Length; i++)
                {
                    entitiesToDamage[i].GetComponent<enemyController>().TakeDamage();
                }
                animator.SetTrigger("Attack");
            }
            //allow attack
            attackDelay = attackDelayStartTime;
        }
        else
        {
            animator.ResetTrigger("Attack");
            attackDelay -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        rigidbody2D.velocity = new Vector2(horiz * speed, rigidbody2D.velocity.y);

        animator.SetFloat("VelY", rigidbody2D.velocity.y);
    }

    //For debugging only
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackBoxX, attackBoxY, 1));
    }
}
