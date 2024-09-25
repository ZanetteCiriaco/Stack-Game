using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] TMP_Text fpsText;
    private float deltaTime = 0.0f;

    private void Start() {
        QualitySettings.vSyncCount = 1;
	    Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
    }

    private void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}
