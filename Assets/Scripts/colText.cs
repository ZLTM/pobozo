using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class colText : MonoBehaviour
{
    public AudioSource sound;
    public GameObject CPanel;

    public List<string> text = new List<string>();
    public int TextCount = 0;
    [SerializeField]//para asignar el siguiente valor en el engine
    private Text ChatWindowNpc = null;
    private bool first = true;

    void OnCollisionStay(Collision collision)//esto revisa CADA frame, no sobrecargar
    {
       
            if (Input.GetKeyDown(KeyCode.E))
            {
                talking();//para diferenciar acciones (?)
            }
        

    }

    void talking()
    {
        //muestra el texto y define la logica de los dialogos
        if (TextCount < text.Count)
        {

            CPanel.SetActive(true);

            //detiene al jugador, esta cosa tomo 2 horas y aun esta bugueada!
            PjController.moving = false;
            ChatWindowNpc.text = text[TextCount];//reemplazo de message.cs, lleva el texto a la ventana
            TextCount++;


        }
        else
        {
            print("enter else");
            //regresa el movimiento y reproduce audio, esto debe variar al colocarce audio por secciones
            ChatWindowNpc.text = "";//limpia la caja de texto
            if (sound != null)
                sound.Play();
            PjController.moving = true;

            CPanel.SetActive(false);
            first = false;

        }
    }
}
