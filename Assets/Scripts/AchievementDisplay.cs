using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class AchievementDisplay : MonoBehaviour {

    public string achievementName;
    public Image image;
    public TMP_Text name;
    public TMP_Text description;
    public GameObject shadow;

    private void Start() {
        Achievement result = DataManager.instance.data.achievement.Where(a => a.name == achievementName).FirstOrDefault();

        image.sprite = Resources.Load<Sprite>($"AchievementsSprites/{result.imageName}");
        name.text = result.name;
        description.text = result.description;
        shadow.SetActive(!result.unlocked);
    }

}
