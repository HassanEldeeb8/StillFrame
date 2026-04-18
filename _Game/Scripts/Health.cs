using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthBarFill;

    private Animator anim;
    private ArcherAI archer;
    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponentInChildren<Animator>();
        archer = GetComponent<ArcherAI>();

        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateBar();

        if (currentHealth > 0)
        {
            if (archer != null)
                archer.Hurt();
            else if (anim != null)
                anim.SetTrigger("hurt");
        }
        else
        {
            isDead = true;

            if (archer != null)
                archer.Die();
            else if (anim != null)
                anim.SetBool("IsDead", true);
        }
    }

    void UpdateBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount =
                Mathf.Clamp01((float)currentHealth / maxHealth);
        }
    }
}