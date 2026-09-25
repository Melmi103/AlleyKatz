using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] GameObject player;
    [SerializeField] public int RangedDamage = 5;
    [SerializeField] Transform firePoint;
    [SerializeField] public float fireRate = 1f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float lifeTime = 5f;
    [SerializeField] GameObject lastSpawnedProjectile;
    [SerializeField] GameObject projectilePrefab;
    public float nextFireTime = 0f;
    private Rigidbody2D rb;
    private Transform playerTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else if (player != null)
        {
            Debug.Log("Found player object");
        }
    }

    void Start()
    {
    }

   
    public void Init(Vector2 direction, float speed)
    {
        if (rb != null)
            rb.linearVelocity = direction.normalized * speed;
    }

    public void RangeShootAtPlayer()
    {
        Vector3 aimDir = player.transform.position - firePoint.position;
        aimDir.z = 0f;
        aimDir.Normalize();


        lastSpawnedProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile proj = lastSpawnedProjectile.GetComponent<Projectile>();
        proj.Init(aimDir, projectileSpeed);

        Debug.Log("Shot!");

        Destroy(lastSpawnedProjectile, lifeTime);

    

  

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("Projectile hit the player!");
            Destroy(gameObject);
        }
    }


}