using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEngine.UI;



public class DayNightCycle : MonoBehaviour {

    public float tick;
    public float seconds;
    public int mins;
    public int hours = 8;
    public int days = 0;


    public static int SECONDS_IN_MIN = 60;
    public static int MINS_IN_HOUR = 60;
    public static int HOURS_IN_DAY = 24;
    public int SUN_SET_START_TIME = 18;
    public int SUN_SET_DURATION = 2;
    public int SUN_RISE_START_TIME = 6;
    public int SUN_RISE_DURATION = 2;

    //public TextMeshProUGUI timeDisplay;
    //public TextMeshProUGUI dayDisplay;
    public Volume postProcessingVolume;


    // Start is called before the first frame update
    void Start() {
        postProcessingVolume = gameObject.GetComponent<Volume>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        CalculateTime();
        DisplayTime();
    }

    private void CalculateTime() {
        seconds += Time.fixedDeltaTime * tick;
        if (seconds >= 60) {
            seconds -= 60;
            mins += 1;
        } if (mins >= 60) {
            mins -= 60;
            hours += 1;
        } if (hours >= 24) {
            hours -= 24;
            days += 1;
        }

        ControlLighting();
    }

    public void ControlLighting() {

        if (hours >= SUN_SET_START_TIME && hours < SUN_SET_START_TIME + SUN_SET_DURATION )
        {
            postProcessingVolume.weight =  ((float) ((hours - SUN_SET_START_TIME) * MINS_IN_HOUR + mins)) / (SUN_SET_DURATION * MINS_IN_HOUR); 
        }
     
        if (hours >= SUN_RISE_START_TIME && hours < SUN_RISE_START_TIME + SUN_RISE_DURATION) {
            postProcessingVolume.weight = 1 - ((float) ((hours - SUN_RISE_START_TIME) * MINS_IN_HOUR + mins)) / (SUN_RISE_DURATION * MINS_IN_HOUR);
        }
    }

    private void DisplayTime() {
        Debug.Log(string.Format("Day {0}, Hours {1}, Mins {2}, Seconds {3}", days, hours, mins, seconds));
    }
}
