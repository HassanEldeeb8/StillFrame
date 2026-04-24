using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthBarFill;

    private ArcherAI archer;
    private MeleeEnemy meleeEnemy;

    public bool IsDead => isDead; 

    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        archer = GetComponent<ArcherAI>();
        meleeEnemy = GetComponent<MeleeEnemy>();

        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateBar();

        if (currentHealth > 0)
        {
            if (archer != null)
                archer.Hurt();

            if (meleeEnemy != null)
                meleeEnemy.Hurt();
        }
        else
        {
            isDead = true;
            GetComponent<Animator>().SetBool("isdead", true);

            if (archer != null)
                archer.Die();

            if (meleeEnemy != null)
                meleeEnemy.Die();
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