using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {

    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text         uitLevel;   // The UiText_Level Text
    public Text         uitShots;   // The UiText_Shots Text
    public Text         uitButton;  // The UiButton_View Text
    public Vector3      castlePos;  // The place to put castles
    public GameObject[] castles;    // An array of the castles

    [Header("Set Dynamically")]
    public int          level;      // Current level
    public int          levelMax;   // number of levels
    public int          shotsTaken;
    public GameObject   castle;     // current castle
    public GameMode     mode = GameMode.idle;
    public string       showing = "Show Slingshot"; // FollowCam mode


	// Use this for initialization
	void Start () {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists
        if (castle!=null) {
            Destroy(castle);
        }

        // destroy old projectiles if they exist
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject item in projectiles) {
            Destroy(item);
        }

        // instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // reset the Goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI() {
        // show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update() {
        UpdateGUI();

        // check for level end
        if ((mode==GameMode.playing) && Goal.goalMet) {
            // change mode to stop checking for level end
            mode = GameMode.levelEnd;
            // zoom out
            SwitchView("Show Both");
            // start next level in 2 seconds
            Invoke("NextLevel", 2f);            
        }
    }

    void NextLevel() {
        level++;

        if (level == levelMax) {
            level = 0;
        }

        StartLevel();
    }

    public void SwitchView(string eView = "") {
        if (eView == "") {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing) {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    // static method that allows code anywhere to increment shotsTaken
    public static void ShotsFired() {
        S.shotsTaken++;
    }
	












	
}
