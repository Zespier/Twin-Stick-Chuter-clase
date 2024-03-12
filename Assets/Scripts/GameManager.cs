using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("HUD")]
    public Animator waveAnimator;
    public TMP_Text waveNumber;

    [Header("Enemies")]
    public string[] enemyList;
    public Transform[] spawnPoints;

    [Header("Waves")]
    public float spawnDelay = 02f;
    public int waveEnemyNumberMultiplier = 20;
    private int waveEnemies;
    private int remainingEnemies;
    public int currentWave = 0;
    private float spawnTimer = 0f;
    public int maxEnemiesOnScene = 15;
    private int enemiesOnScene = 0;

    public static GameManager instance;
    private void Awake() {
        if (!instance) { instance = this; }
    }

    private void OnEnable() {
        EnemyHealth.OnDead += EnemyDead;
    }

    private void Start() {
        NewWave();
    }

    private void Update() {
        if (spawnTimer < spawnDelay) { spawnTimer += Time.deltaTime; }

        if (waveEnemies > 0 && spawnTimer > spawnDelay && enemiesOnScene < maxEnemiesOnScene) {
            GenerateEnemy(enemyList);
            spawnTimer = 0;
        }

        if (remainingEnemies <= 0) {
            NewWave();
        }
    }

    private void OnDisable() {
        EnemyHealth.OnDead -= EnemyDead;
    }

    public void GenerateEnemy(string[] enemyPool) {
        if (spawnPoints.Length == 0 || enemyPool.Length == 0) {
            Debug.LogError("Te has olvidado de configurar los spawns o los enemigos");
            return;
        }

        int randomSpawn = Random.Range(0, spawnPoints.Length);
        int randomEnemy = Random.Range(0, enemyPool.Length);

        PoolManager.instance.Pull(enemyPool[randomEnemy], spawnPoints[randomSpawn].position, Quaternion.identity);

        waveEnemies--;
        enemiesOnScene++;
    }

    public void EnemyDead() {
        remainingEnemies--;
        enemiesOnScene++;
    }

    public void NewWave() {
        currentWave++;
        waveNumber.text = currentWave.ToString();
        waveAnimator.SetTrigger("Show");
        waveEnemies = currentWave * waveEnemyNumberMultiplier;
        remainingEnemies = waveEnemies;

        enemiesOnScene = 0;
    }
}
