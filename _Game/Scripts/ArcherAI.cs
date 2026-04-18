using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    public Transform player;
    public GameObject arrowPrefab;
    public Transform firePoint;

    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float shootCooldown = 1.5f;

    private Animator anim;
    private Rigidbody2D rb;

    private float shootTimer = 0f;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        shootTimer -= Time.fixedDeltaTime;

        float dx = player.position.x - transform.position.x;
        float absDx = Mathf.Abs(dx);
        float dirX = dx > 0 ? 1 : -1;

        // Face player
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dirX;
        transform.localScale = scale;

        if (absDx > attackRange)
        {
            Vector2 newPos = rb.position + new Vector2(dirX * moveSpeed * Time.fixedDeltaTime, 0f);
            rb.MovePosition(newPos);
            anim.SetFloat("Speed", 1f);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);

            if (shootTimer <= 0f)
            {
                Shoot(dirX);
                shootTimer = shootCooldown;
            }
        }
    }

    void Shoot(float dirX)
    {
        if (isDead || firePoint == null || arrowPrefab == null) return;

        anim.SetTrigger("Attack");

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow a = arrow.GetComponent<Arrow>();
        if (a != null)
            a.SetDirection(new Vector2(dirX, 0f));
    }

    public void Hurt()
    {
        if (isDead) return;
        anim.SetTrigger("hurt");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        anim.SetBool("IsDead", true);
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        this.enabled = false;

        Destroy(gameObject, 2f);
    }
}