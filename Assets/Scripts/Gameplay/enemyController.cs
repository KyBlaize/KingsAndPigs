using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float speed, randomSpeedMin, randomSpeedMax;
    public bool right;

    [SerializeField] private Collider2D collider;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2D;
    private Vector2 direction;
    private CameraShake cameraShake;
    private LayerMask layer;
    private float baseSpeed;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
    }

    private void OnEnable()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        layer = 11; //Hard code
        speed = baseSpeed + Random.Range(randomSpeedMin, randomSpeedMax);
    }

    void Update()
    {
        if (!right) //Change to interface?
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            direction = Vector2.left;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(direction.x * speed, rigidbody2D.velocity.y);

        //in case they spawn outside the space
        if (transform.position.y < -6.5)
        {
            animator.SetTrigger("Death");
            StartCoroutine(Wait());
        }
    }

    public void TakeDamage()
    {
        speed = 0;
        layer = 9;
        animator.SetTrigger("Death");
        cameraShake.Shake();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
