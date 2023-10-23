using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpForce = 5.7f;
    [SerializeField] private float climbSpeed = 0.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private GameObject UI;

    private Vector3 startScale;
    private bool onPlatform = false;
    private bool isGrounded = true;
    private bool isClimbing = false;
    private GameObject currentPlatform;
    private Ground currentGround = null;

    private Vector2 startColliderOffset;
    private Vector2 startColliderSize;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        startScale = transform.localScale;

        startColliderOffset = playerCollider.offset;
        startColliderSize = playerCollider.size;
    }

    private void Update()
    {
        bool jumpInput = Input.GetButtonDown("Jump");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(startScale.x * -1, startScale.y, startScale.z);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = startScale;
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && onPlatform)
        {
            Fall(true);
        }

        if (transform.position.y <= -4.03 || (currentGround is not null && currentGround.groundPosition <= transform.position.y))
        {
            isGrounded = true;
        }

        if (isClimbing)
        {
            transform.position = transform.position + new Vector3(horizontalInput * climbSpeed * Time.deltaTime, 0, 0);
            if (jumpInput)
            {
                TurnClimbingMode(false);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Vector2 moveDirection = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            rb.velocity = moveDirection;
            if (jumpInput && isGrounded)
            {
                Debug.Log($"[PlayerController] Jump");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsClimbing", isClimbing);
    }

    private IEnumerator DisableCollision()
    {
        playerCollider.isTrigger = true;
        yield return new WaitForSeconds(0.4f);
        playerCollider.isTrigger = false;
    }

    private IEnumerator DisableCollisionWithPlatform()
    {
        BoxCollider2D platformCollider = currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void Fall(bool isFallingFromPlatform)
    {
        if (isFallingFromPlatform)
        {
            StartCoroutine(DisableCollisionWithPlatform());
        }
        else
        {
            StartCoroutine(DisableCollision());
        }

        onPlatform = false;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onPlatform = collision.gameObject.CompareTag("Platform");
        if (collision.gameObject.CompareTag("Ground") || onPlatform)
        {
            Debug.Log($"[PlayerController] Collision {collision.gameObject.CompareTag("Ground")} {onPlatform}");
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("IsClimbing", isClimbing);
            animator.SetBool("IsGrounded", isGrounded);
            if (onPlatform)
            {
                currentPlatform = collision.gameObject;
            }
            currentGround = collision.gameObject.GetComponent<Ground>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"[PlayerController] OnCollisionExit2D");
        bool isPlatform = collision.gameObject.CompareTag("Platform");
        if (collision.gameObject.CompareTag("Ground") || isPlatform)
        {
            isGrounded = false;
        }
        if (isPlatform)
        {
            onPlatform = false;
        }
        currentGround = null;
    }

    private void TurnClimbingMode(bool type)
    {
        isClimbing = type;
        animator.SetBool("IsClimbing", isClimbing);
        rb.gravityScale = type ? 0.0f : 1.0f;
        if (type)
        {
            playerCollider.offset = new Vector2(-0.015f, 0.012f);
            playerCollider.size = new Vector2(0.3f, 0.93f);

            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            playerCollider.offset = startColliderOffset;
            playerCollider.size = startColliderSize;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[PlayerController] OnTriggerEnter2D");
        if (other.CompareTag("Exit"))
        {
            UI.GetComponent<GameManager>().ShowWin();
        }
        else if (other.CompareTag("Clothes"))
        {
            transform.position = new Vector3(transform.position.x, other.GetComponent<Clothes>().ropeYCoord - 0.7f, transform.position.z);
            TurnClimbingMode(true);
        }
        else if (other.CompareTag("BarrelCat"))
        {
            Fall(false);
            rb.velocity = new Vector2(0, -1);
        }
        else if (other.CompareTag("KillingEnemy"))
        {
            UI.GetComponent<GameManager>().ShowLoss();
        }
        else if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            UI.GetComponent<GameManager>().AddPoint();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[PlayerController] OnTriggerExit2D");
        if (other.CompareTag("Clothes") && isClimbing)
        {
            TurnClimbingMode(false);
        }
    }
}
