using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour {

    //------------Initialization----------------

    //------------Gameplay----------------
    public static Action OnStartEnemySpawner;
    public static Action OnWaveStarted;

    public static Action<Vector2> OnTargetMove;

}
