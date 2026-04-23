using System;
using UnityEngine;
//This script manages the player's weapon state, sprites, animations, interactions with pickups, health and communicates weapon changes to other components.


public class FistsBehaviour : MonoBehaviour
{
    [Header("Player Stats and Settings")]

    [SerializeField] public int playerhealth = 100;
    public enum WeaponType {         
        Fists,
        Knife,
        Hatchet
    }

    [SerializeField] private WeaponType currentWeapon = WeaponType.Fists;
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
        Fist();
    }

 
    void Weapon()
    {
        switch (currentWeapon)
        {
            case WeaponType.Fists:
                Fist();
                break;
            case WeaponType.Knife:
                Knives();
                break;
            case WeaponType.Hatchet:
                Hatchet();
                break;
            default:
                Debug.Log("No weapon selected. Switching to default.");
                break;
        }
    }

    void Fist()
    {

            // Fist-specific behavior (Sprites, anims, etc)
            SetWeapon(WeaponType.Fists);

    }

  void Knives()
    {
      
            
        // Knife-specific behavior (Sprites, anims, etc)
                SetWeapon(WeaponType.Knife);

    }
    void Hatchet()
    {
       
                SetWeapon(WeaponType.Hatchet);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("KnifeCrate"))
        {
            Debug.Log("Knife crate hit. Switching to Knife.");
            Knives();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("HatchetCrate"))
        {
            Debug.Log("Hatchet crate hit. Switching to Hatchet.");
            Hatchet();
            Destroy(collision.gameObject);
        }
    }

    void Update()
    {
        Weapon();
        //Debug.Log ("Current weapon in FistsBehaviour: " + currentWeapon);
    }
}
