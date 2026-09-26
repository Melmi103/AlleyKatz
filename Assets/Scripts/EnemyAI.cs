using JetBrains.Annotations;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Main Enemy Stats and Settings")]

    [SerializeField] public int meleeenemyHealth = 120;
    [SerializeField] public int rangedenemyHealth = 80;
    [SerializeField] public GameObject player;
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject shot;
    private Transform playerTransform;


    [Header("Melee Enemy Settings")]
    [SerializeField] private float detectionRange = 0.0f;
    [SerializeField] private bool isTopDown = true;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private int MeleeDamage = 20;
    [SerializeField] private float angleoffset = 4.0f;
   

    private int rotateddirection = 0;
    private bool lockedon = false;
    private float timer = 1f;

    [Header("Ranged Enemy Settings")]
    [SerializeField] private float minSafeDistance = 5f;
    [SerializeField] private float fleeSpeed = 5f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private Animator animator;
   

    public float newFireRate;
    public float newFireTime;



    [Header("Xtras")]
    [SerializeField] private bool useSpriteFlip = true;
    [SerializeField] private SpriteRenderer visualSprite;

    private void Awake()
    {
    
        
            if (shot == null)
            {
                shot = Resources.Load<GameObject>("Yarn");
            }
            else if (shot != null)
            {
                Debug.Log("Found Shot object");
            }

        if (projectile == null)
        {
            
            if (shot != null)
            {
                projectile = shot.GetComponent<Projectile>();
            }
       
            if (projectile != null)
            {
                newFireRate = projectile.GetComponent<Projectile>().fireRate;
                newFireTime = projectile.GetComponent<Projectile>().nextFireTime;
                Debug.Log("Found projectile component");
            }

            if (player == null)
            {
                var Player = GameObject.FindWithTag("Player");
                if (Player != null)
                {
                    player = Resources.Load<GameObject>("Player");
                }
                else return;

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
        //Debug.Log("Melee Enemy");
        switch (currentState)
        {
            case EnemyState.Idle:
                if (Vector3.Distance(transform.position, player.transform.position) < detectionRange)
                {
                    AimAtPlayer();
                    if (lockedon == true)
                    {
                        SetState(EnemyState.Attack);
                    }
                }

                break;
            case EnemyState.Attack:

                transform.position += transform.up * 12f * Time.deltaTime;

                StartCoroutine(ResetStateAfterSeconds(1f));
                lockedon = false;
                timer = 1f;
                break;

        }
    }
    void RangedState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
              //  Debug.Log("RangedEnemy is Idle!");
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
                // Debug.Log("RangedEnemy is Attacking!");
                if (Time.time >= newFireTime)
                {
                    newFireTime = Time.time + 1f / newFireRate;
                    projectile.GetComponent<Projectile>().RangeShootAtPlayer();
                }
                FacePlayer();
                if (Vector3.Distance(transform.position, player.transform.position) < minSafeDistance)
                {
                    SetState(EnemyState.Flee);
                   
                
                }
                animator.SetBool("IsRunning", false);
                break;
            case EnemyState.Flee:
                if (Vector3.Distance(transform.position, player.transform.position) > minSafeDistance)
                {
                 

                    SetState(EnemyState.Idle);
                  //Debug.Log("RangedEnemy is Idle!");
               }
                else
                {
                   // Debug.Log("RangedEnemy is Fleeing!");
                    FleeFromPlayer();
                }
                animator.SetBool("IsRunning", true);
                break;

        }
    }

 
}
