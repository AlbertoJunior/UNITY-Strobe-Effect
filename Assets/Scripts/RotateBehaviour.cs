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
    public float timeScaledFlashStrobe = 1;

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
        rate = Time.fixedDeltaTime * timeScaledFlashStrobe;

        frameRateText.text = "Frame Rate: " + 1 / Time.fixedDeltaTime;
        timeText.text = "Time Refresh: " + Time.fixedDeltaTime;
        automaticText.text = "Automatic: " + automaticDegree;
        maxValueText.text = "Max Degree: " + maxValueDegree;
        rotationText.text = "Rotation: " + transform.rotation.z;
        actuallyValue.text = "Actually: " + rotateDegree;
        strobeText.text = "Strobe Active: " + strobeActive;
        delayFlashText.text = "Delay Flash: " + timeScaledFlashStrobe + " (" + 1/rate + ")";

        rps = Mathf.Abs((rotateDegree / Time.fixedDeltaTime) / 360f);
        rpsText.text = "RPS/Hz: " + rps;
    }

    public void inputS() {
        strobeActive = !strobeActive;
        strobeText.text = "Strobe Active: " + strobeActive;

        //strobe off
        if (!strobeActive) {
            //hide object
            objStrobe.SetActive(false);
            flashOn = false;
        }
    }

    public void InputSpace() {
        start = !start;
    }

    public void InputRShift() {
        automaticDegree = !automaticDegree;
        automaticText.text = "Automatic: " + automaticDegree;
    }

    public void InputLShift() {
        strobeConfig = !strobeConfig;
        strobeConfigText.text = "L Shift: Strobe Config (" + strobeConfig + ")";
    }

    public void InputUpdateValue(int value) {
        if (strobeConfig) {
            float auxV = value / 10f;
            timeScaledFlashStrobe += auxV;
            if (timeScaledFlashStrobe < 0)
                timeScaledFlashStrobe = 0;
        }
        else if (automaticDegree) {
            maxValueDegree += value;
        }
        else {
            rotateDegree += value;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            InputSpace();
    
        if (Input.GetKeyDown(KeyCode.S)) 
            inputS();
        

        if (Input.GetKeyDown(KeyCode.RightShift)) 
            InputRShift();
        

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
            InputLShift();


        if (Input.GetKeyDown(KeyCode.RightArrow))
            InputUpdateValue(10);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            InputUpdateValue(-10);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            InputUpdateValue(1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            InputUpdateValue(-1);

        if (Input.GetKeyDown(KeyCode.Keypad1))
            rotateDegree = 90f;
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            rotateDegree = 180f;
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            rotateDegree = 270f;
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            rotateDegree = 360f;
        else if (Input.GetKeyDown(KeyCode.Keypad5))
            rotateDegree = 450f;
    }

    void FixedUpdate() {
        rps = Mathf.Abs((actuallyDegree / Time.fixedDeltaTime) / 360f);

        rpsText.text = "RPS/Hz: " + rps;
        frameRateText.text = "Frame Rate: " + 1 / Time.fixedDeltaTime;
        maxValueText.text = "Max Degree: " + maxValueDegree;
        timeText.text = "Time Refresh: " + Time.fixedDeltaTime;

        if (strobeActive) {
            rate = Time.fixedDeltaTime * timeScaledFlashStrobe;
            delayFlashText.text = "Delay Flash: " + timeScaledFlashStrobe + " (" + 1/rate + ")";

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
