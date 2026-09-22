using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats and Settings")]

    [SerializeField] public int enemyHealth = 100;
    [SerializeField] private float detectionRange = 0.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isTopDown = true;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float angleoffset = 4.0f;

    private int rotateddirection = 0;
    private bool lockedon = false;
    private float timer = 1f;


    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
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
        return enemyHealth;
    }

    public void SetHealth(int newHealth)
    {
        enemyHealth = newHealth;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        SetState(EnemyState.Idle);
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
            case EnemyState.Patrol:
                break;
            case EnemyState.Chase:

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
       // Debug.Log("Ranged Enemy");
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Patrol:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;

        }
    }

 
}
