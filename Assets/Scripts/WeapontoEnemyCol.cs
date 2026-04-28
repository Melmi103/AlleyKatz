using System;
using UnityEngine;
// This script manages detection of weaponry with enemies, damage application/calucations and hitboxes.

public class WeapontoEnemyCol : MonoBehaviour
{
     private FistsBehaviour fistsBehaviour;
     private FistsBehaviour.WeaponType currentWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        fistsBehaviour = GetComponent<FistsBehaviour>() ?? GetComponentInParent<FistsBehaviour>();
        if (fistsBehaviour == null)
        {
            Debug.LogError("FistsBehaviour component not found in the current GameObject or its parents.");
            return;
        }

        currentWeapon = fistsBehaviour.GetCurrentWeapon;
        fistsBehaviour.OnWeaponChanged += HandleWeaponChanged;
    }

    private void HandleWeaponChanged(FistsBehaviour.WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log("Current weapon updated to: " + currentWeapon);
    }

    private void OnDestroy()
    {
        if (fistsBehaviour != null)
            fistsBehaviour.OnWeaponChanged -= HandleWeaponChanged;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Within WEAPONSTOCOL.CS the current weapon is identified as: " + currentWeapon);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with an enemy while wielding: " + currentWeapon);
           
        }
    }
}
