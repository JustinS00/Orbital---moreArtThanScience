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
    public bool night;
    public static DayNightCycle instance;


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

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start() {
        instance.postProcessingVolume = gameObject.GetComponent<Volume>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        CalculateTime();
        DisplayTime();
    }

    private void CalculateTime() {
        instance.seconds += Time.fixedDeltaTime * tick;
        if (instance.seconds >= 60) {
            instance.seconds %= 60;
            instance.mins += 1;
        } if (instance.mins >= 60) {
            instance.mins %= 60;
            instance.hours += 1;
        } if (instance.hours >= 24) {
            instance.hours %= 24;
            instance.days += 1;
        }

        ControlLighting();
    }

    public void ControlLighting() {

        if (instance.hours >= SUN_SET_START_TIME && instance.hours < SUN_SET_START_TIME + SUN_SET_DURATION )
        {
            instance.postProcessingVolume.weight =  ((float) ((instance.hours - SUN_SET_START_TIME) * MINS_IN_HOUR + instance.mins)) / (SUN_SET_DURATION * MINS_IN_HOUR); 
            instance.night = true;
        }
     
        if (instance.hours >= SUN_RISE_START_TIME && instance.hours < SUN_RISE_START_TIME + SUN_RISE_DURATION) {
            instance.postProcessingVolume.weight = 1 - ((float) ((instance.hours - SUN_RISE_START_TIME) * MINS_IN_HOUR + instance.mins)) / (SUN_RISE_DURATION * MINS_IN_HOUR);
            instance.night = false;
        }

    }

    public void DisplayTime() {
        //Debug.Log(string.Format("Day {0}, Hours {1}, Mins {2}, Seconds {3}", days, hours, mins, seconds));
    }

    public bool isNight() {
        return instance.night;
    }
}
