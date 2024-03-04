using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable/Shake", fileName = "SHAkeEEeeeeeeEeeeeEEEeeeeeee")]
public class ShakeScriptable : ScriptableObject {

    public NoiseSettings noiseType;
    public AnimationCurve amplitude;
    public AnimationCurve frequency;
    [Range(0, 4)] public float shakeDuration = 2;

    private void OnValidate() {
    }
}

public class ZespierTools {
    public static void Method1() {

    }
}


