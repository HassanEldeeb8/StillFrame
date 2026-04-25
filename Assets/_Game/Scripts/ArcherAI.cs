using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject arrowPrefab;
    public Transform firePoint;
    private float lastDir;

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
        shootTimer = 0f;

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

        shootTimer -= Time.deltaTime;

        float dx = player.position.x - transform.position.x;
        float distToPlayer = Mathf.Abs(dx);
        float dir = dx > 0 ? 1f : -1f;

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
            if (anim) anim.SetFloat("Speed", 0f);
        }

        transform.position = pos;
    }

    void LockInsideSpawnZone()
    {
        Vector3 pos = transform.position;

        float minX = spawnPos.x - maxDistanceFromSpawn;
        float maxX = spawnPos.x + maxDistanceFromSpawn;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        transform.position = pos;
    }

    void Flip(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = dir > 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Shoot(float dir)
    {
        if (anim)
            anim.SetTrigger("Attack");

        lastDir = dir;

        CancelInvoke(nameof(SpawnArrow));
        Invoke(nameof(SpawnArrow), 0.44f);
    }
    void SpawnArrow()
    {
        if (arrowPrefab == null || firePoint == null)
            return;

        Vector3 spawnPos = new Vector3(
            firePoint.position.x + (lastDir * 0.3f),
            firePoint.position.y - 0.15f,
            firePoint.position.z
        );

        GameObject arrow =
            Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

        Arrow a = arrow.GetComponent<Arrow>();

        if (a != null)
            a.SetDirection(new Vector2(lastDir, 0f));
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
        CancelInvoke();
        Destroy(gameObject, 2f);
    }
}