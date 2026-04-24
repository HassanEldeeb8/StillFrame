using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2.5f;
    public float chaseRange = 8f;

    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float attackDuration = 0.6f;
    public int damage = 15;

    public Transform attackPoint;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private float attackTimer;
    private bool isDead;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("Animator on: " + anim.gameObject.name);
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null)
            return;

        if (isAttacking)
            return;

        attackTimer -= Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange && attackTimer <= 0f)
        {
            StartCoroutine(AttackRoutine());
            return;
        }

        if (dist <= chaseRange)
        {
            if (dist >= 1)
                Chase();
            else
                Idle();
        }
        else
            Idle();
        Debug.Log("Distance = " + dist);
        Debug.Log("ATTACKING");
    }

    void Chase()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        anim.SetBool("isMoving", true);

        Vector3 s = transform.localScale;

        if (dir.x > 0)
            s.x = -Mathf.Abs(s.x);
        else
            s.x = Mathf.Abs(s.x);

        transform.localScale = s;
    }

    void Idle()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("isMoving", false);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isMoving", false);

        // direct clip play instead of state transition
        //anim.runtimeAnimatorController = null;
        //anim.Play("attack");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDuration);

        DoDamage();

        attackTimer = attackCooldown;

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    public void DoDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            playerLayer
        );

        if (hit != null)
        {
            Health hp = hit.GetComponent<Health>();

            if (hp != null)
                hp.TakeDamage(damage);
        }
    }

    public void Hurt()
    {
        if (isDead) return;

        anim.Play("hurt");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        anim.Play("die");

        Destroy(gameObject, 2f);
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}