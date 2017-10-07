using UnityEngine;
using System.Collections;

public class levelManager : MonoBehaviour {
    public string levelToLoad;
    public AudioSource soundsource;
    public AudioClip sound;
    public bool go = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(AdvancedInteract.pass == 1 && Application.loadedLevelName == "Train")
        {
            Application.LoadLevel("ToyBox");
        }
        	
	}
   
    void OnCollisionStay(Collision collision)//esto revisa CADA frame, no sobrecargar
    {
        if (go)
        {
            Application.LoadLevel("Train");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Application.loadedLevelName == "SchoolInterior" && this.gameObject.name == "out")
            {
                Application.LoadLevel("School");
            }

            else if(Application.loadedLevelName == "School" && this.gameObject.name == "ChangeLevel")
            {                    
                soundsource.clip = sound;
                if (sound != null)
                {
                    StartCoroutine(WaitForSeconds());
                }                 
               
            }
        }

    }

    IEnumerator WaitForSeconds()
    {

        soundsource.Play();
        PjController.moving = false;
        yield return new WaitForSeconds(1);
        PjController.moving = true;
        go = true;
    }

}
