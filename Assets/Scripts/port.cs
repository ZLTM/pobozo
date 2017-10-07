using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class port : MonoBehaviour {

    public GameObject target;
    public Vector3 portplace;
    public AudioSource soundsource;
    public AudioClip sound;

    void OnCollisionStay(Collision collision)//esto revisa CADA frame, no sobrecargar
    {       

        if (Input.GetKeyDown(KeyCode.E))
        {
            porting();
            soundsource.clip = sound;
            if (sound != null)
                soundsource.Play();

        }

    }

    public void porting()
    {
        print("porting");
        target.transform.position = portplace;
    }
}
