using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GMScript : MonoBehaviour {

    private GameObject normCanvas;
    private GameObject conCanvas;

    public GameObject Cam;
    public GameObject CombatCam;
    public GameObject statsPanel;
    public GameObject DeathPan; 
    public int batCount = 0;

    public AudioSource mback;
    public AudioSource bback;
    //public GameObject choicePanel;
    //public GameObject targetPanel;
    public static bool InBattle = HeroSM.inBattle;
    public static bool isDead;

    public MovieTexture yatiriMovie;
    public MovieTexture trainAbduction;
    public MovieTexture meetMartha;    
    public static MovieTexture Ending;

    public static MovieTexture moviePlaying;
    public AudioSource aud;
    public GameObject Confade;
    public GameObject gamfade;
    private int i = 0;
    private bool entertut;


    /*
    void awake()//en caso de usar start para realisar una accion, eliminar la logica de cinematics y activar awake
    {
        print(Application.loadedLevelName);
        if(Application.loadedLevelName == "ToyBox" && yatiriMovie != null)
        {
            print("playing yatiri");
            moviePlaying = yatiriMovie;          
        }
        else if (Application.loadedLevelName == "JaenStreet" && trainAbduction != null)
        {
            moviePlaying = trainAbduction;
        }
        else if (Application.loadedLevelName == "elViajero" && meetMartha != null)
        {
            moviePlaying = meetMartha;
        }     

        if (moviePlaying != null)
        {
            moviePlaying.Play();
            Screen.fullScreen = true;
        }
    }
    */

	// Use this for initialization
	void Start () {
        normCanvas = GameObject.Find("Canvas");
        conCanvas = GameObject.Find("BattleCanvas");
        Confade = GameObject.Find("BattleCanvas/CombatFade");
        gamfade = GameObject.Find("Canvas/GameFade");
        entertut = true;
        Confade.SetActive(false);
        gamfade.SetActive(false);

        if (Application.loadedLevelName == "SchoolInterior" && yatiriMovie != null)
        {
            moviePlaying = yatiriMovie;
        }
        else if (Application.loadedLevelName == "Train" && trainAbduction != null)
        {
            moviePlaying = trainAbduction;
        }
        else if (Application.loadedLevelName == "School" && moviePlaying != null)
        {
            moviePlaying = null;
        }
        else if (Application.loadedLevelName == "ToyBox" && meetMartha != null)
        {
            moviePlaying = meetMartha;
        }

        if (moviePlaying != null)
        {
            aud = GetComponent<AudioSource>();
            aud.clip = moviePlaying.audioClip;
            moviePlaying.Play();           
            aud.Play();
            Screen.fullScreen = true;
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (moviePlaying != null)
        {
            if (moviePlaying.isPlaying == true)
            {
                mback.mute = true;
                bback.mute = true;
            }

            if (moviePlaying.isPlaying == false)
            {
                mback.mute = false;
                bback.mute = false;
            }
        }

        if (gamfade != null)
        {
            if (Application.loadedLevelName == "SchoolInterior" && entertut)
            {
                gamfade.SetActive(true);
                i = 2;
            }

            if (i == 2 && Input.GetKeyDown(KeyCode.E))
            {
                gamfade.SetActive(false);
                entertut = false;
            }
        }

        if (Confade != null)
        {
            if (InBattle && i == 0)
            {

                Confade.SetActive(true);

            }

            if (InBattle == true && Input.GetKeyDown(KeyCode.E))
            {
                Confade.SetActive(false);
                i = 1;
            }
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Screen.fullScreen = false;
            moviePlaying.Stop();
        }

        if (isDead)
        {
            DeathPan.SetActive(true);
        }


        InBattle = HeroSM.inBattle;

        if (InBattle == true)
        {            
            normCanvas.SetActive(false);
            conCanvas.SetActive(true);
           
            AdvancedInteract.Chating = false;
            //targetPanel.active = true;   //esta siendo manejado desde bs como enemyselect        
            statsPanel.SetActive(true);
            //choicePanel.active = true;
            
            PjController.moving = false;
            Cam.SetActive (false);
            CombatCam.SetActive(true);
            if (i == 0 && conCanvas)
            {
                Confade.SetActive(true);
            }
            batCount++;
        }
        else if (InBattle == false)
        {
            normCanvas.SetActive(true);
            conCanvas.SetActive(false);
            AdvancedInteract.Chating = true;
            //targetPanel.active = false;
            statsPanel.SetActive(false);
            //choicePanel.active = false;

            if (AdvancedInteract.Chating == false)
            {
                PjController.moving = true;
            }
            Cam.SetActive(true);
            CombatCam.SetActive(false);
        }

    }

    void OnGUI()
    {
        if(moviePlaying != null && moviePlaying.isPlaying)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), moviePlaying, ScaleMode.StretchToFill);
        }
    }
}
