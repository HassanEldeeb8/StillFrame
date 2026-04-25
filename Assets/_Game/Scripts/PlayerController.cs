using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    public float idleFreezeDelay = 0.25f;

    private Rigidbody2D rb;
    private Animator anim;
    private float move;
    private bool isGrounded;
    private float idleTimer;

    public Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (anim != null)
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (health != null && health.IsDead)
        {
            Time.timeScale = 1f;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
        bool attackPressed = Input.GetMouseButtonDown(0);
        bool parryPressed = Input.GetMouseButtonDown(1);

        move = Input.GetAxisRaw("Horizontal");

        bool hasInput =
            move != 0 ||
            jumpPressed ||
            attackPressed ||
            parryPressed;

        if (hasInput)
        {
            Time.timeScale = 1f;
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.unscaledDeltaTime;

            if (idleTimer >= idleFreezeDelay)
                Time.timeScale = 0f;
        }

        anim.SetFloat("Speed", Mathf.Abs(move));

        Vector3 s = transform.localScale;

        if (move > 0)
            s.x = Mathf.Abs(s.x);
        else if (move < 0)
            s.x = -Mathf.Abs(s.x);

        transform.localScale = s;

        if (jumpPressed && isGrounded)
        {
            Time.timeScale = 1f;

            rb.linearVelocity =
                new Vector2(rb.linearVelocity.x, jumpForce);

            isGrounded = false;
            anim.SetBool("IsJumping", true);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity =
            new Vector2(move * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("IsJumping", true);
        }
    }
}