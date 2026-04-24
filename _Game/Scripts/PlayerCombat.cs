using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public int attackDamage = 20;
    public LayerMask enemyLayer;

    private Animator anim;
    private bool isAttacking;
    private Health health;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>(); 
    }

    void Update()
    {
        if (health != null && health.IsDead) return;
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        if (anim != null)
            anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.1f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D col in hits)
        {
            Health hp = col.GetComponentInParent<Health>();
            if (hp != null)
                hp.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
