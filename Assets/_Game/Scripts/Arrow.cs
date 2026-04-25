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

        Vector3 s = transform.localScale;

        s.x = direction.x > 0 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);

        transform.localScale = s;
    }

    void Start()
        {
            Collider2D myCol = GetComponent<Collider2D>();

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject e in enemies)
            {
                Collider2D enemyCol = e.GetComponent<Collider2D>();

                if (enemyCol != null && myCol != null)
                    Physics2D.IgnoreCollision(myCol, enemyCol);
            }

            Destroy(gameObject, 5f);
        }

        void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
                return;

            if (col.CompareTag("Player"))
            {
                PlayerCombat pc = col.GetComponent<PlayerCombat>();
                if (pc != null && pc.IsParrying())
                {
                    Debug.Log("ARROW PARRIED");
                    Destroy(gameObject);
                    return;
                }

                Health hp = col.GetComponent<Health>();

                if (hp != null)
                    hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }