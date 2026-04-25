using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator anim;
    private float move;
    private bool isGrounded;
    public Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (health != null && health.IsDead)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop movement
            return;
        }
        // Movement input
        move = Input.GetAxisRaw("Horizontal");

        // Animation
        anim.SetFloat("Speed", Mathf.Abs(move));

        // Face direction
        Vector3 s = transform.localScale;

        if (move > 0)
            s.x = Mathf.Abs(s.x);
        else if (move < 0)
            s.x = -Mathf.Abs(s.x);

        transform.localScale = s;


        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("IsJumping", true);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
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
