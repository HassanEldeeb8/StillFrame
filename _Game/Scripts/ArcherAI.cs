using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject arrowPrefab;
    public Transform firePoint;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 8f;
    public float attackRange = 5f;
    public float stopDistance = 4f;

    [Header("Spawn Lock")]
    public float maxDistanceFromSpawn = 3f;

    [Header("Attack")]
    public float shootCooldown = 1.5f;

    private Animator anim;
    private float shootTimer;
    private bool isDead = false;
    private bool canThink = false;

    private Vector3 spawnPos;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }

        spawnPos = transform.position;
        shootTimer = shootCooldown;

        Invoke(nameof(EnableAI), 0.2f);
    }

    void EnableAI()
    {
        canThink = true;
    }

    void Update()
    {
        if (isDead || player == null || !canThink)
            return;

        float dx = player.position.x - transform.position.x;
        float distToPlayer = Mathf.Abs(dx);
        float dir = dx > 0 ? 1f : -1f;

        shootTimer -= Time.deltaTime;

        if (distToPlayer > chaseRange)
        {
            ReturnToSpawn();
            return;
        }

        Flip(dir);

        if (distToPlayer > stopDistance)
        {
            Move(dir);

            if (anim) anim.SetFloat("Speed", 1f);
        }
        else
        {
            if (anim) anim.SetFloat("Speed", 0f);

            if (distToPlayer <= attackRange && shootTimer <= 0f)
            {
                Shoot(dir);
                shootTimer = shootCooldown;
            }
        }

        LockInsideSpawnZone();
    }

    void Move(float dir)
    {
        Vector3 pos = transform.position;

        pos.x += dir * moveSpeed * Time.deltaTime;

        float minX = spawnPos.x - maxDistanceFromSpawn;
        float maxX = spawnPos.x + maxDistanceFromSpawn;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        pos.y = spawnPos.y;
        pos.z = spawnPos.z;

        transform.position = pos;
    }

    void ReturnToSpawn()
    {
        Vector3 pos = transform.position;

        float dx = spawnPos.x - pos.x;

        if (Mathf.Abs(dx) > 0.05f)
        {
            float dir = dx > 0 ? 1f : -1f;

            Flip(dir);

            pos.x += dir * moveSpeed * Time.deltaTime;

            float minX = spawnPos.x - maxDistanceFromSpawn;
            float maxX = spawnPos.x + maxDistanceFromSpawn;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);

            if (anim) anim.SetFloat("Speed", 1f);
        }
        else
        {
            pos.x = spawnPos.x;

            if (anim) anim.SetFloat("Speed", 0f);
        }

        pos.y = spawnPos.y;
        pos.z = spawnPos.z;

        transform.position = pos;
    }

    void LockInsideSpawnZone()
    {
        Vector3 pos = transform.position;

        float minX = spawnPos.x - maxDistanceFromSpawn;
        float maxX = spawnPos.x + maxDistanceFromSpawn;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = spawnPos.y;
        pos.z = spawnPos.z;

        transform.position = pos;
    }

    void Flip(float dir)
    {
        Vector3 scale = transform.localScale;

        scale.x = dir > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void Shoot(float dir)
    {
        if (arrowPrefab == null || firePoint == null)
            return;

        if (anim) anim.SetTrigger("Attack");

        GameObject arrow =
            Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow a = arrow.GetComponent<Arrow>();

        if (a != null)
            a.SetDirection(new Vector2(dir, 0f));
    }

    public void Hurt()
    {
        if (isDead) return;

        if (anim) anim.SetTrigger("hurt");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim) anim.SetBool("IsDead", true);

        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 2f);
    }
}