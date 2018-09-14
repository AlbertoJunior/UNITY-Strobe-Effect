using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBehaviour : MonoBehaviour {
    public int frameRate;
    public float rotateDegree;

    public bool start;
    public bool automaticDegree;

    public GameObject objStrobe;
    public bool strobeActive;
    [Tooltip("Flash per frame")]
    public int timeScaledFlashStrobe = 1;

    public float actuallyDegree;
    public float maxValueDegree;
    public float rps = 0;

    public Text automaticText, actuallyValue, maxValueText, rotationText, frameRateText, timeText, rpsText;

    private float contFrameTime = 0;
    private float rate;
    private bool flashOn = false;

    private void Awake() {
        //determinate the frame rate
        Application.targetFrameRate = frameRate;
        //disable vSync for optimize accurate effect
        QualitySettings.vSyncCount = 0;
    }

    // Use this for initialization
    void Start() {
        objStrobe.SetActive(flashOn);

        frameRateText.text = "Frame Rate: " + 1 / Time.fixedDeltaTime;
        timeText.text = "Time Refresh: " + Time.fixedDeltaTime;
        automaticText.text = "Automatic: " + automaticDegree;
        maxValueText.text = "Max Degree: " + maxValueDegree;
        rotationText.text = "Rotation: " + transform.rotation.z;
        actuallyValue.text = "Actually: " + rotateDegree;

        rps = ((1f / Time.fixedDeltaTime) * rotateDegree) / 360f;
        rpsText.text = "RPS: " + rps;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            start = !start;

        if (Input.GetKeyDown(KeyCode.RightShift)) {
            automaticDegree = !automaticDegree;
            automaticText.text = "Automatic: " + automaticDegree;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            strobeActive = !strobeActive;
            automaticText.text = "Automatic: " + automaticDegree;
        }

        if (automaticDegree) {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                maxValueDegree += 10;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                maxValueDegree -= 10;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                maxValueDegree += 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                maxValueDegree -= 1;
        }
        else {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                rotateDegree += 10;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                rotateDegree -= 10;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                rotateDegree += 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                rotateDegree -= 1;
        }
    }

    void FixedUpdate() {
        rps = ((1f / Time.fixedDeltaTime) * actuallyDegree) / 360f;

        rpsText.text = "RPS: " + rps;
        frameRateText.text = "Frame Rate: " + 1 / Time.fixedDeltaTime;
        maxValueText.text = "Max Degree: " + maxValueDegree;
        timeText.text = "Time Refresh: " + Time.fixedDeltaTime;

        if (strobeActive) {
            rate = Time.fixedDeltaTime * timeScaledFlashStrobe;

            if (flashOn) {
                contFrameTime += Time.fixedDeltaTime;

                if (contFrameTime >= rate) {
                    flashOn = false;
                    contFrameTime = 0;
                }
            }
            else {
                flashOn = true;
            }

            objStrobe.SetActive(flashOn);
        }

        if (start) {
            if (automaticDegree) {
                actuallyDegree += 0.05f;
                if (actuallyDegree > maxValueDegree && maxValueDegree > 0) {
                    actuallyDegree = maxValueDegree;
                }
            }
            else {
                actuallyDegree = rotateDegree;
            }

            actuallyValue.text = "Actually: " + actuallyDegree;
            transform.Rotate(Vector3.forward, actuallyDegree);

            rotationText.text = "Rotation: " + transform.rotation.z;
        }
    }
}
