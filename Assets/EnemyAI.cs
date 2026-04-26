using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats and Settings")]

    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private float detectionRange = 0.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private bool isTopDown = true;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float angleoffset = 4.0f;
    [SerializeField] private int damage = 10;

    private int rotateddirection = 0;
    private bool aimed = false;
    private float timer = 0f;


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



    private void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
        }
    }


    void Start()
    {
        SetState(EnemyState.Idle);
    }

   
    void Update()
    {
        Enemy();
        if (currentState == EnemyState.Chase)
        {
            AimAtPlayer();
        }
    }

    void AimAtPlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (timer < 1f)
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
                timer = timer + 1;
                aimed = true;
            }

        }


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
                    SetState(EnemyState.Chase);
                }

                break;
            case EnemyState.Patrol:
                break;
            case EnemyState.Chase:
                if (aimed)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < 10f)
                    {
                        SetState(EnemyState.Attack);
                    }
                }

                break;
            case EnemyState.Attack:

                gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.forward, 9f * Time.deltaTime);
                
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
