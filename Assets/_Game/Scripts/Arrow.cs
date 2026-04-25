using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        if (dir.x > 0)
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Start()
    {
        Collider2D myCol = GetComponent<Collider2D>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemies)
        {
            Collider2D[] enemyCols = e.GetComponentsInChildren<Collider2D>();

            foreach (Collider2D enemyCol in enemyCols)
            {
                if (enemyCol != null && myCol != null)
                    Physics2D.IgnoreCollision(myCol, enemyCol);
            }
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
            return;

        PlayerCombat pc = col.GetComponentInParent<PlayerCombat>();
        Health hp = col.GetComponentInParent<Health>();

        if (pc != null && pc.TryParry())
        {
            Debug.Log("ARROW PARRIED");
            Destroy(gameObject);
            return;
        }

        if (col.CompareTag("Player"))
        {
            if (hp != null)
                hp.TakeDamage(damage);

            Destroy(gameObject);
            return;
        }

        if (!col.isTrigger)
            Destroy(gameObject);
    }
}