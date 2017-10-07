using UnityEngine;
using System.Collections;

public class ChangeSound : MonoBehaviour {

    public AudioSource soundSource;
    public AudioClip tr;
    public AudioClip co;
    private int i;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider trigger)
    {
        if (i == 0)
        {
            soundSource.clip = co;
            i = 1;
            soundSource.Play();
        }
        else if (i == 1)
        {
            soundSource.clip = tr;
            i = 0;
            soundSource.Play();
        }
    }
}
