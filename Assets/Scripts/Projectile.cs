using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] GameObject player;
    [SerializeField] public int RangedDamage = 5;
    [SerializeField] Transform firePoint;
    [SerializeField] float lifetime = 5f;
    [SerializeField] public float projectileSpeed = 1;
    [SerializeField] public int damage = 20;

    private AudioSource audioSource;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private float sfxVolume = 1.0f;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private PlayerData PlayerData;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            player = Resources.Load<GameObject>("Player");
        }
        else if (player != null)
        {
            Debug.Log("Found player object");
        }

        if (PlayerData == null)
        {
            PlayerData = player.GetComponent<PlayerData>();
        }
        else if (PlayerData != null)
        {
            Debug.Log("Found PlayerHealth component");
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; 
        }

    }


    void Start()
    {
    }

   
    public void Init(Vector3 direction, float speed)
    {
        if (rb != null)
            rb.linearVelocity = direction.normalized * speed;

        if (shotSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(shotSFX, sfxVolume);
        }
    }

  
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player"))
       // {

       //     Debug.Log("Projectile hit the player!");
       //     PlayerData.SetHealth(PlayerData.GetHealth() - RangedDamage);

      //      Destroy(gameObject);
       // }
    }


}