using UnityEngine;
using System.Collections;

public class MyoOverlay : MonoBehaviour {

    public GameObject myo = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ThalmicHub hub = myo.GetComponent<ThalmicHub>();
        if (Input.GetKeyDown(KeyCode.Q))
            hub.ResetHub();
    }

    void OnGUI () {
        GUI.skin.label.fontSize = 20;

        ThalmicHub hub = ThalmicHub.instance;

        // Access the ThalmicMyo script attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        if (!hub.hubInitialized) {
            GUI.Label(new Rect(12, 8, Screen.width, Screen.height),
                "Cannot contact Myo Connect. Is Myo Connect running?\n" +
                "Press Q to try again."
            );
        }
        else if (!thalmicMyo.isPaired) {
            GUI.Label(new Rect(12, 8, Screen.width, Screen.height),
                "No Myo currently paired."
            );
        }
        else if (!thalmicMyo.armSynced) {
            GUI.Label(new Rect(12, 8, Screen.width, Screen.height),
                "Perform the Sync Gesture."
            );
        }
        else  {
            GUI.Label(new Rect(12, 8, Screen.width, Screen.height),
               "Ready."
            );
        }
    }
}
