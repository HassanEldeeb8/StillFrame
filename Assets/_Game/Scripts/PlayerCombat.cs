using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public int attackDamage = 20;
    public LayerMask enemyLayer;

    public float parryDuration = 0.4f;
    public float parryCooldown = 0.6f;

    private Animator anim;
    private bool isAttacking;
    private bool isParrying;
    private bool canParry = true;

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
            StartCoroutine(Attack());

        if (Input.GetMouseButtonDown(1) && canParry)
            StartCoroutine(Parry());
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        if (anim) anim.SetTrigger("Attack");

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

    IEnumerator Parry()
    {
        canParry = false;
        isParrying = true;

        if (anim) anim.SetTrigger("Parry");

        yield return new WaitForSeconds(parryDuration);

        isParrying = false;

        yield return new WaitForSeconds(parryCooldown);

        canParry = true;
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}