using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats and Settings")]

    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private float detectionRange = 0.0f;
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
        Enemy();
        SetState(EnemyState.Idle);
    }

   
    void Update()
    {
      
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
                
                   
            case EnemyState.Patrol:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
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
