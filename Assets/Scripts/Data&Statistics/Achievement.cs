using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement {

    public string name;
    public string statCode;
    public string imageName;
    public string description;
    public int targetAmount;
    public bool unlocked = false;

}
