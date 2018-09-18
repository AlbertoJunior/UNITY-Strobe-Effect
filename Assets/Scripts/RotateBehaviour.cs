using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBehaviour : MonoBehaviour {
    public int frameRate;
    public float rotateDegree;

    public bool start;
    public bool automaticDegree;
    public bool strobeConfig;

    public GameObject otherCylinder;
    public GameObject objStrobe;
    public bool strobeActive;
    [Tooltip("Flash per frame")]
    public int timeScaledFlashStrobe = 1;

    public float actuallyDegree;
    public float maxValueDegree;
    public float rps = 0;

    public Text automaticText, actuallyValue, maxValueText, rotationText, frameRateText, timeText, rpsText;
    public Text strobeText, delayFlashText, strobeConfigText;

    private float contFrameTime = 0;
    private float rate;
    private bool flashOn = false;

    private void Awake() {
        //setting the time of update frames
        Time.fixedDeltaTime = 1f / frameRate;
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

        rps = (rotateDegree / Time.fixedDeltaTime) / 360f;
        rpsText.text = "RPS/Hz: " + rps;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            start = !start;

        if (Input.GetKeyDown(KeyCode.S)) {       
            strobeActive = !strobeActive;
            strobeText.text = "Strobe Active: " + strobeActive;
            
            //strobe off
            if (!strobeActive) {
                //hide object
                objStrobe.SetActive(false);
                flashOn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightShift)) {
            automaticDegree = !automaticDegree;
            automaticText.text = "Automatic: " + automaticDegree;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            strobeConfig = !strobeConfig;
            strobeConfigText.text = "L Shift: Strobe Config (" + strobeConfig + ")";
        }

        if (strobeConfig) {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                timeScaledFlashStrobe += 10;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                timeScaledFlashStrobe -= 10;
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                timeScaledFlashStrobe += 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                timeScaledFlashStrobe -= 1;
        }
        else if (automaticDegree) {
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

        rpsText.text = "RPS/Hz: " + rps;
        frameRateText.text = "Frame Rate: " + 1 / Time.fixedDeltaTime;
        maxValueText.text = "Max Degree: " + maxValueDegree;
        timeText.text = "Time Refresh: " + Time.fixedDeltaTime;

        if (strobeActive) {
            rate = Time.fixedDeltaTime * timeScaledFlashStrobe;
            delayFlashText.text = "Delay Flash: " + timeScaledFlashStrobe + " (" + rate + ")";

            //is showing object
            if (flashOn) {
                //update count time variable
                contFrameTime += Time.fixedDeltaTime;

                if (contFrameTime >= rate) {       
                    //hide object
                    flashOn = false;
                    //reset count time variable
                    contFrameTime = 0;
                }
            }
            else {
                //show object
                flashOn = true;
            }

            //setting view state of object
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

            //update actually degree rotation
            actuallyValue.text = "Actually: " + actuallyDegree;
            transform.Rotate(Vector3.forward, actuallyDegree);
            //update rotation of other cylinder
            otherCylinder.transform.rotation = transform.rotation;
            //update actually rotation Z of object
            rotationText.text = "Rotation: " + transform.rotation.z;
        }
    }
}
