using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // 🔥 Fix zero direction
        if (direction == Vector2.zero)
            direction = Vector2.right;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) return;

        if (col.CompareTag("Player"))
        {
            Health hp = col.GetComponent<Health>();
            if (hp != null)
                hp.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}