using NUnit.Framework.Internal;
using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Main Enemy Stats and Settings")]

    [SerializeField] public int meleeenemyHealth = 120;
    [SerializeField] public int rangedenemyHealth = 80;
    [SerializeField] public GameObject player;
    [SerializeField] public Projectile projectile;
    [SerializeField] public GameObject shot;
    private Transform playerTransform;


    [Header("Melee Enemy Settings")]
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private bool isTopDown = true;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private int MeleeDamage = 20;
    [SerializeField] private float angleoffset = 4.0f;
    private float MeleeSpeed;
    private float MeleeRandomDistance;
    private Vector3 meleeTargetPosition;


    private int rotateddirection = 0;
    private bool lockedon = false;
    private float timer = 1f;

    [Header("Ranged Enemy Settings")]
    [SerializeField] private float minSafeDistance = 10f;
    [SerializeField] private float fleeSpeed = 5f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] public GameObject lastSpawnedProjectile;
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject ShotPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float lifeTime = 5f;
   
   
    public Transform firePoint;
    public float newFireRate = 1f;
    public float newFireTime = 0f;
   


    private void Awake()
    {

        if (projectile == null)
        {
            
            if (shot != null)
            {
                projectile = shot.GetComponent<Projectile>();
                
               
            }
       
            if (projectile != null)
            {
                //newFireRate = projectile.GetComponent<Projectile>().fireRate;
                //newFireTime = projectile.GetComponent<Projectile>().nextFireTime;
                Debug.Log("Found projectile component");

            }
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            if (player != null)
            {
                Debug.Log("Found player object!,");
            }

            
        }
    }





public enum EnemyState
    {
        Idle,
        Attack,
        Flee,
    }
    public enum EnemyType
    {
        melee,
        ranged
    }



    [SerializeField] private EnemyState currentState;
    [SerializeField] private EnemyType currentType;

    private event Action<EnemyState> OnStateChanged;
    public event Action<int> OnHealthChanged;



    private void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
        }
    }



    public int GetHealth()
    {
        return currentType switch
        {
            EnemyType.ranged => rangedenemyHealth,
            EnemyType.melee => meleeenemyHealth,
            _ => throw new InvalidOperationException($"Unknown EnemyType: {currentType}")
        };
    }
    

    //health setters
    public void SetHealth(int newHealth)
    {
        if (currentType == EnemyType.melee)
        {
            meleeenemyHealth = newHealth;
            if (meleeenemyHealth <= 0)
            {
                Die();
            }
        }
        if (currentType == EnemyType.ranged)
        {
            rangedenemyHealth = newHealth;
            if (rangedenemyHealth <= 0)
            {
                Die();
            }
        }
    }
    public void RangeShootAtPlayer()
    {
        Vector3 aimDir = player.transform.position - firePoint.position;
        aimDir.z = 0f;
        aimDir.Normalize();


        lastSpawnedProjectile = Instantiate(ShotPrefab, firePoint.position, Quaternion.identity);

        Projectile proj = lastSpawnedProjectile.GetComponent<Projectile>();
        proj.Init(aimDir, projectileSpeed);

        Debug.Log("Shot!");

        Destroy(lastSpawnedProjectile, lifeTime);





    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            if (currentType == EnemyType.melee)
            {
               meleeenemyHealth -= collision.GetComponent<Projectile>().damage;
                OnHealthChanged?.Invoke(meleeenemyHealth);
                if (meleeenemyHealth <= 0) 
                {
                    Die();
                }
            }
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        SetState(EnemyState.Idle);
        Debug.Log("Melee Enemy Health: " + meleeenemyHealth);
        Debug.Log("Ranged Enemy Health: " + rangedenemyHealth);
    }

   
    void Update()
    {
        Enemy();
        

    }


    void AimAtPlayer()
    {
        
        Vector3 dir = player.transform.position - transform.position;
        if (timer == 1f)
        {

            if (isTopDown)
            {
                float baseangle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                float angle = baseangle + angleoffset;
                Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

                if (smoothRotation)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                else
                    transform.rotation = targetRot;

                StartCoroutine(LockOnAfterSeconds(0.5f));   

            }

        }


    }

    void FleeFromPlayer()
    {
        Vector3 fleeDir = transform.position - player.transform.position;
        fleeDir.z = 0f;
        fleeDir.Normalize();

        transform.position += fleeDir * fleeSpeed * Time.deltaTime;

        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }


    }

    void FacePlayer()
    {
        new WaitForSeconds(0.5f);

        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x > player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

  

    private IEnumerator LockOnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lockedon = true;
    }

    private IEnumerator ResetStateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetState(EnemyState.Idle);
    }


    // Enemy type and state management. (What they do when in the state)
    void Enemy()
    {
        
        if (gameObject.CompareTag("MeleeEnemy"))
            currentType = EnemyType.melee;


        else if (gameObject.CompareTag("RangedEnemy"))
            currentType = EnemyType.ranged;

        switch (currentType)
        {

            case EnemyType.melee:
                MeleeState();
                break;
            case EnemyType.ranged:
                RangedState();
                break;
        }

    }



    void MeleeState()
    {
        if (player == null) return;

        switch (currentState)
        {
            case EnemyState.Idle:
              
                //if (detectionRange <= 0f || Vector3.Distance(transform.position, player.transform.position) < detectionRange)
               // {
                    MeleeRandomDistance = Randomizer.CreateRandomizer().NextFloat(0.5f, 50f);

                  
                    Vector3 dirToPlayer = player.transform.position - transform.position;
                    dirToPlayer.z = 0f;
                    meleeTargetPosition = transform.position + dirToPlayer.normalized * MeleeRandomDistance;

                    AimAtPlayer();

                    if (lockedon)
                        SetState(EnemyState.Attack);
               // }
                break;

            case EnemyState.Attack:
             
                MeleeSpeed = Randomizer.CreateRandomizer().NextFloat(5f, 20f);

                Vector3 toTarget = meleeTargetPosition - transform.position;
                toTarget.z = 0f;
                float dist = toTarget.magnitude;

             
                if (dist > 0.1f)
                {
                    transform.position += toTarget.normalized * MeleeSpeed * Time.deltaTime;
                    //Debug.DrawLine(transform.position, meleeTargetPosition, Color.red, 0.1f);
                }
                else
                {
                    SetState(EnemyState.Idle);
                }

                StartCoroutine(ResetStateAfterSeconds(1f));
                lockedon = false;
                timer = 1f;
                break;
        }
    }
    void RangedState()
    {
        //Debug.Log(
    //"Enemy: " + gameObject.name +
   // " | State: " + currentState +
    //" | Distance: " + Vector3.Distance(transform.position, player.transform.position) +
   // " | Safe Distance: " + minSafeDistance +
   // " | Attack Range: " + attackRange);


        switch (currentState)
        {
            
            case EnemyState.Idle:
            //Debug.Log("RangedEnemy is Idle!");
            if (Vector3.Distance(transform.position, player.transform.position) < minSafeDistance)
            {
                SetState(EnemyState.Flee);
            }
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange && Vector3.Distance(transform.position, player.transform.position) > minSafeDistance)
            {
                SetState(EnemyState.Attack);
            }
            animator.SetBool("IsRunning", false);
            break;
        case EnemyState.Attack:
            //Debug.Log("RangedEnemy is Attacking!");
            if (Time.time >= newFireTime)
            {
                newFireTime = Time.time + 1f / newFireRate;
                RangeShootAtPlayer();


                GameObject spawned = Instantiate(shot, transform.position, Quaternion.identity);
                var spawnedProj = spawned.GetComponent<Projectile>();
                if (spawnedProj != null)
                {
                    Vector3 aimDir = player.transform.position - transform.position;
                    aimDir.z = 0f;
                    aimDir.Normalize();
                    spawnedProj.Init(aimDir, spawnedProj.projectileSpeed);
                }
                else
                {
                    Debug.LogWarning("EnemyAI.Attack: spawned shot missing Projectile component.");
                }
            }

            FacePlayer();
            if (Vector3.Distance(transform.position, player.transform.position) < minSafeDistance)
            {
                SetState(EnemyState.Flee);
                //Debug.Log("Enemy is fleeing!!");

            }
            animator.SetBool("IsRunning", false);
            break;
        case EnemyState.Flee:

          //  Debug.Log("Distance: " + Vector3.Distance(transform.position, player.transform.position));
                if (Vector3.Distance(transform.position, player.transform.position) > minSafeDistance)
            {

                //Debug.Log("RangedEnemy is Fleeing!");
                FleeFromPlayer();
            }
            animator.SetBool("IsRunning", true);
            break;
        
        }
    }

 
}
