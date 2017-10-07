using UnityEngine;
using System.Collections;
//aqui se debe cambiar el dialogo segun las circunstancias
public class ChangeStat : MonoBehaviour {

    public GameObject victim;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D coli)
    {
        AdvancedInteract.calling = 2;
    }
}
