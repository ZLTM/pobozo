using UnityEngine;
using System.Collections;
//supa cam V2.0!!!
public class CamFollow : MonoBehaviour
{
    public bool stop;
    private Vector3 minCamPos;
    private Vector3 maxCamPos;

    public float distance;//distancia en eje z (altura), "sube en negativo"
    public Transform target;
    public float smoothTime = 0.3f;//porcentaje de  velocidad al que la camara sigue al jugador
    //public float size = 0.5170351f; //activar y reemplazar en el valor de 0.7 en train para debugear

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
       

        if(Application.loadedLevelName == "School")
        {
            minCamPos = new Vector3 (-1.7f, -0.78f, -1000000f);
            maxCamPos = new Vector3(1.62f, 0.91f, 1000000f);
        }
        else if (Application.loadedLevelName == "SchoolInterior")
        {
            Camera.main.orthographicSize = 0.85f;
            minCamPos = new Vector3(-4.5f, -3.0f, -1000000f);
            maxCamPos = new Vector3(4.3f, 3.1f, 1000000f);
        }
        else if (Application.loadedLevelName == "Train")
        {

            Camera.main.orthographicSize = 0.7f;
            minCamPos = new Vector3(-3.48f, 0.1f, -1000000f);
            maxCamPos = new Vector3(2.45f, 0.1f, 1000000f);
        }
        else if (Application.loadedLevelName == "ToyBox")
        {
            minCamPos = new Vector3(-0.6f, -1.72f, -1000000f);
            maxCamPos = new Vector3(0.7f, -0.2f, 1000000f);
        }
    }

    void Update()
    {
      
            Vector3 goalPos = target.position;//(0, 0, 0)

            goalPos.z = distance;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);//sigue al target definiendose como (posicion actual, destino, velocidad, smooth)   
            if(stop)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCamPos.x, maxCamPos.x),
                Mathf.Clamp(transform.position.y, minCamPos.y, maxCamPos.y),
                Mathf.Clamp(transform.position.z, minCamPos.z, maxCamPos.z));
        }     
    } 

}