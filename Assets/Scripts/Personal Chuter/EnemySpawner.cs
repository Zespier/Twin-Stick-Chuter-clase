using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float initialMoney = 1000f;
    public float moneyPerSecond = 100f;
    public float timeBetweenWaves = 3f;
    public List<Wave> waves = new List<Wave>();

    private int _currentWave;
    private float _currentMoney;
    private float _waveTimer;
    private List<Coroutine> c_ActiveWaves = new List<Coroutine>();

    private void Update() {
        _waveTimer += Time.deltaTime;
    }

    //private float MoneyPerSecondThisWave() {
    //    return Mathf.Lerp(moneyPerSecond, moneyPerSecond * 2, (float)_currentWave / waves);
    //}

    private void CheckForSpawnableWaves() {

        if ((_waveTimer >= timeBetweenWaves && waves[_currentWave].startType == StartType.additive)) {
            c_ActiveWaves.Add(StartCoroutine(C_SpawnWave(waves[_currentWave])));
        }
    }

    private IEnumerator C_SpawnWave(Wave wave) {

        switch (wave.waveType) {
            case WaveType.continuos:



                break;
            case WaveType.instantaneous:
                SpawnWaveInstantaneous(wave);
                yield break;
            default:
                SpawnWaveInstantaneous(wave);
                yield break;
        }

        yield return null;
    }

    private void SpawnWaveInstantaneous(Wave wave) {
        for (int i = 0; i < wave.amount; i++) {
            //TODO: SPAWN ALL ENEMIES TROUGH EVERY SPAWNER => POSITIONS WILL COME AFTER THAT
        }
    }
}

[System.Serializable]
public class Wave {
    public WaveType waveType;
    public StartType startType;
    public EnemyType enemyType;
    public float amount;
}

public enum WaveType {
    continuos,
    instantaneous
}

public enum StartType {
    additive,
    exclusive
}

public enum EnemyType {
    normal,
    fast,
    heavy,
    miniboss,
    boss
}