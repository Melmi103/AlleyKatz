using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
//This script manages the player's weapon state, sprites, animations, interactions with pickups, health and communicates weapon changes to other components.


public class PlayerData : MonoBehaviour
{
    [Header("Player Stats and Settings")]

    [SerializeField] public int playerhealth = 100;
    public enum WeaponType {        
        Yarn,
        Fire,
        Ice
    }

    [SerializeField] private WeaponType currentWeapon = WeaponType.Yarn;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject EnemyRanged;
    [SerializeField] private GameObject EnemyMelee;
    [SerializeField] public GameObject shot;
    [SerializeField] private float lifetime = 5f;
   

    public event Action<WeaponType> OnWeaponChanged;
    public event Action<int> OnHealthChanged;

    public WeaponType GetCurrentWeapon => currentWeapon;

    void Awake()
    {
        if (EnemyRanged == null)
        {
            EnemyRanged = GameObject.FindGameObjectWithTag("RangedEnemy");
        }
        else if (EnemyRanged != null)
        {
            Debug.Log("Found Ranged Enemy object");
        }
        if (EnemyMelee == null)
        {
            EnemyMelee = GameObject.FindGameObjectWithTag("MeleeEnemy");
        }
        else if (EnemyMelee != null)
        {
            Debug.Log("Found Melee Enemy object");
        }
        if (shot == null)
        {
            shot = GameObject.FindGameObjectWithTag("Yarn");
        }
        else if (shot != null)
        {
            Debug.Log("Found Shot");
        }    

    }

    private void SetWeapon(WeaponType newWeapon)
    {
           if (currentWeapon != newWeapon)
        {
            currentWeapon = newWeapon;
            OnWeaponChanged?.Invoke(currentWeapon);
        }
    }
    public int GetHealth()
    {
        return playerhealth;
    }

    public int SetHealth(int newHealth)
    {
        playerhealth = newHealth;
        OnHealthChanged?.Invoke(playerhealth);
        return playerhealth;
    }


    void Start()
    {
        YarnBall();
        
    }

 
    void Weapon()
    {
        switch (currentWeapon)
        {
            case WeaponType.Yarn:
                YarnBall();
                break;
            case WeaponType.Fire:
                Fire();
                break;
            case WeaponType.Ice:
                Ice();
                break;
            default:
                Debug.Log("No weapon selected. Switching to default.");
                break;
        }
    }

    void YarnBall()
    {

            // Fist-specific behavior (Sprites, anims, etc)
            SetWeapon(WeaponType.Yarn);

    }

  void Fire()
    {
      
            
        // Knife-specific behavior (Sprites, anims, etc)
                SetWeapon(WeaponType.Fire);

    }
    void Ice()
    {
       
                SetWeapon(WeaponType.Ice);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
     
    }
    void Shoot()
    {
        Debug.Log("Shoot function called!");

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 aimDir = mouseWorldPos - hand.transform.position;
        aimDir.z = 0f;
        aimDir.Normalize();

       
        Vector3 spawnPos = hand.transform.position + aimDir * 0.5f;
        GameObject spawned = Instantiate(shot, spawnPos, Quaternion.identity);

        var spawnedProj = spawned.GetComponent<Projectile>();
        if (spawnedProj != null)
        {
            spawnedProj.Init(aimDir, spawnedProj.projectileSpeed);
            Debug.Log("Shot spawned");
        }
        Destroy(spawned, lifetime);
    }

    void Update()
    {
        Weapon();

      
        bool clicked = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || Input.GetMouseButtonDown(0);
        if (clicked)
        {
            Shoot();
        }
    }
}
