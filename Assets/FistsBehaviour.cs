using System;
using UnityEngine;

public class FistsBehaviour : MonoBehaviour
{
    private enum WeaponType {         
        Fists,
        Knife,
        Hatchet
    }

    [SerializeField] private WeaponType currentWeapon = WeaponType.Fists;
    [SerializeField] private GameObject hand; 

    void Start()
    {

        currentWeapon = WeaponType.Fists;
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
                break;
            default:
                Debug.Log("No weapon selected. Switching to default.");
                break;
        }
    }       
     

    void Fist()
    {
      if (gameObject.CompareTag("Fists"))
      {
        Debug.Log("Fists are active.");
            return;
        }

    }

  void Knives()
    {
        if (gameObject.CompareTag("Knife"))
        {
            Debug.Log("Knife is active.");
            return;
        }
 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("KnifeCrate"))
        {
            Debug.Log("Knife crate hit. Switching to Knife.");
            currentWeapon = WeaponType.Knife;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("HatchetCrate"))
        {
            Debug.Log("Hatchet crate hit. Switching to Hatchet.");
           currentWeapon = WeaponType.Hatchet;
            Destroy(collision.gameObject);
        }

    }

    void Update()
    {
        Weapon();
    }
}
