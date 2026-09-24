using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject enemyMelee;
    [SerializeField] private GameObject enemyRanged;

    [SerializeField] private float MeleeswarmerInterval = 3.5f;
    [SerializeField] private float RangedswarmerInterval = 5.0f;
    [SerializeField] private float buffer = 1.5f;
 
    void Start()
    {
       StartCoroutine(spawnMeleeEnemy(MeleeswarmerInterval, enemyMelee));
       StartCoroutine(spawnRangedEnemy(RangedswarmerInterval, enemyRanged));
    }

    // Update is called once per frame
    private IEnumerator spawnMeleeEnemy(float interval, GameObject enemyMelee)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemyMelee, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        StartCoroutine(spawnMeleeEnemy(interval, enemyMelee));
    }
    private IEnumerator spawnRangedEnemy(float interval, GameObject enemyRanged)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemyRanged, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
        StartCoroutine(spawnRangedEnemy(interval, enemyRanged));
    }
}
