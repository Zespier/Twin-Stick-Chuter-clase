using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownMissile : CooldownUI {

    public override void OnEnable() {
        base.OnEnable();
        PlayerController.OnShootMissile += StartTimer;
    }

    public override void OnDisable() {
        base.OnDisable();
        PlayerController.OnShootMissile += StartTimer;
    }

}
