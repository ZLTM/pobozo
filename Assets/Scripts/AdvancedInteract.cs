using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AdvancedInteract : MonoBehaviour {

    public AudioSource MusicSource;
    public AudioClip backGround;
    public AudioClip Batle;
    public static bool Chating = true;

    public AudioSource mback;
    public AudioSource bback;
    
    public List<string> text = new List<string>();
    public List<string> text2 = new List<string>();
    public List<string> text5 = new List<string>();
    public List<string> batleText = new List<string>();
    public static int TextCount = 0;
    public static int TextCount2 = 0;
    public static int TextCount5 = 0;
    public static int BatleTextCount = 0;
    public static int pass = 0;

    [SerializeField]//para asignar el siguiente valor en el engine
    private Text ChatWindowNpc = null;
    public GameObject CPanel;
    public static int calling = 1;//llamado a dialogo

    public MovieTexture mart;
    public AudioSource aud;

    // Use this for initialization
    void Start()
    {
        

    }


    void OnCollisionStay(Collision trigger)//esto revisa CADA frame, no sobrecargar
    {
        if (Chating)
        {
            //revisamos si es un enemigo
            if (this.tag == "EnemySprite")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    fight();//iniciamos combate
                }
            }

            else
            {   //revisamos el estado del dialogo
                if (calling == 1)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        talking();//para diferenciar acciones (?)
                    }
                }

                if (calling == 2)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        talking2();//otros dialogos
                    }
                }
                if (calling == 5)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        talking5();//para diferenciar acciones (?)
                    }
                }
            }
        }

    }

    void talking()
    {
        //muestra el texto y define la logica de los dialogos
        if (TextCount < text.Count)
        {
            CPanel.SetActive(true);
            PjController.moving = false;
            ChatWindowNpc.text = text[TextCount];//reemplazo de message.cs, lleva el texto a la ventana
            TextCount++;

        }
        else
        {   //detiene al jugador
            ChatWindowNpc.text = "";//limpia la caja de texto
            PjController.moving = true;
            TextCount = 0;
            Chating = false;
            CPanel.SetActive(false);
        }

    }

    void talking2()
    {

        //este es otro dialogo para el mismo pj!
        if (TextCount2 < text2.Count)
        {
            CPanel.SetActive(true);
            PjController.moving = false;
            ChatWindowNpc.text = text2[TextCount2];
            TextCount2++;

        }
        else
        {
            ChatWindowNpc.text = "";
            PjController.moving = true;
            TextCount2 = 0;
            Chating = false;
            CPanel.SetActive(false);
            
            calling = 1;
        }

    }

    void talking5()
    {
        //muestra el texto y define la logica de los dialogos
        if (TextCount5 < text5.Count)
        {
            CPanel.SetActive(true);
            PjController.moving = false;
            ChatWindowNpc.text = text5[TextCount5];//reemplazo de message.cs, lleva el texto a la ventana
            TextCount5++;

        }
        else
        {   //detiene al jugador
            ChatWindowNpc.text = "";//limpia la caja de texto
            PjController.moving = true;
            TextCount = 0;
            Chating = false;
            CPanel.SetActive(false);
            pass = 1;
        }

    }

    void fight()
    {

        
        //dialogo de combate e inicio de batalla
        if (BatleTextCount < batleText.Count)
        {
            CPanel.SetActive(true);
            //no esta en batalla asi que controlamos el movimiento normalmente
            PjController.moving = false;
            ChatWindowNpc.text = batleText[BatleTextCount];
            BatleTextCount++;
        }
        else
        {
            ChatWindowNpc.text = "";

            aud = GetComponent<AudioSource>();
            aud.clip = mart.audioClip;
            mart.Play();
            aud.Play();
            Screen.fullScreen = true;

            BatleTextCount = 0;
            HeroSM.inBattle = true;
            MusicSource.clip = Batle;
            MusicSource.Play();
            Chating = false;
            CPanel.SetActive(false);
        }
        
    }

    void OnGUI()
    {
        if (mart != null && mart.isPlaying)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mart, ScaleMode.StretchToFill);
        }
    }

    void Update()
    {
        if (mart != null)
        {
            if (mart.isPlaying == true)
            {
                print("muting");
                mback.mute = true;
                bback.mute = true;
            }

            if (mart.isPlaying == false)
            {
                print("not muting");
                mback.mute = false;
                bback.mute = false;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Screen.fullScreen = false;
                mart.Stop();
            }
        }
    }

}
