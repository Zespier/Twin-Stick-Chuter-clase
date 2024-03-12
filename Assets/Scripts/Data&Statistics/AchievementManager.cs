using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AchievementManager : MonoBehaviour {

    public static Action<string, string> OnAchievementUnlocked;

    private void OnEnable() {
        EnemyHealth.OnDead += () => IncreaseStatAndCheckAchievement("kill", 1);
    }
   
    public void IncreaseStatAndCheckAchievement(string code, int amount) {
        Stat stat = DataManager.instance.data.statistics.Where(s => s.code == code).FirstOrDefault();

        if (stat == null) { return; }
        stat.value += amount;

        foreach (Achievement achievement in DataManager.instance.data.achievement.Where(a => a.statCode == code && !a.unlocked).AsEnumerable()) {

            if (stat.value >= achievement.targetAmount) {
                achievement.unlocked = true;
                OnAchievementUnlocked?.Invoke(achievement.name, achievement.imageName);
                DataManager.instance.Save();
            }
        }
    }

}
