using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour {

    public Image imageUI;
    public TMP_Text nameUI;
    public Animator animator;
    public float delayAchievement = 3f;

    private float lastAchievementTime;

    private void OnEnable() {
        AchievementManager.OnAchievementUnlocked += SetAndShow;
    }

    private void OnDisable() {
        AchievementManager.OnAchievementUnlocked += SetAndShow;
    }

    public void SetAndShow(string name, string imageName) {
        if (lastAchievementTime > Time.time) {
            lastAchievementTime += delayAchievement;
        } else {
            lastAchievementTime = Time.time + delayAchievement;
        }

        StartCoroutine(DelayShow(name, imageName, delayAchievement));
    }

    private IEnumerator DelayShow(string name, string imageName, float showTime) {

        while (Time.time < showTime) {
            yield return null;
        }

        nameUI.text = name;
        imageUI.sprite = Resources.Load<Sprite>(imageName);
        animator.SetTrigger("Show");
    }

}
