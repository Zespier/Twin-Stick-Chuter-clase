using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour {

    public Image shadow;

    private Coroutine timer;

    public virtual void OnEnable() {

    }

    public virtual void OnDisable() {

    }

    private void Start() {
        shadow.fillAmount = 0;
    }

    private IEnumerator Timer(float time) {
        float counter = time;

        while (counter > 0) {

            shadow.fillAmount = counter / time;

            counter -= Time.deltaTime;
            yield return null;
        }

        shadow.fillAmount = 0;
    }

    protected void StartTimer(float time) {

        if (timer != null) { StopCoroutine(timer); }

        timer = StartCoroutine(Timer(time));
    }

}
