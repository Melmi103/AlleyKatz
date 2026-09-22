using System;
using UnityEngine;
//This script manages the player's weapon state, sprites, animations, interactions with pickups, health and communicates weapon changes to other components.


public class FistsBehaviour : MonoBehaviour
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

    public event Action<WeaponType> OnWeaponChanged;
    public event Action<int> OnHealthChanged;

    public WeaponType GetCurrentWeapon => currentWeapon;

    private void SetWeapon(WeaponType newWeapon)
    {
           if (currentWeapon != newWeapon)
        {
            currentWeapon = newWeapon;
            OnWeaponChanged?.Invoke(currentWeapon);
        }
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
        if (collision.gameObject.CompareTag("KnifeCrate"))
        {
            Debug.Log("Crate 2 hit. Switching to Fire.");
            Fire();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("HatchetCrate"))
        {
            Debug.Log("Crate 3 hit. Switching to Ice.");
            Ice();
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        Weapon();
        //Debug.Log ("Current weapon in FistsBehaviour: " + currentWeapon);
    }
}
