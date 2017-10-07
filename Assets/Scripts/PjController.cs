using UnityEngine;
using System.Collections;

public class PjController : MonoBehaviour {

    public float Speed = 0f;
    public float movex = 0f;
    public float movey = 0f;
    Rigidbody body;
    public static bool moving = true;
    Animator Anim;
    

    // Use this for initialization
    void Start () {
        Anim = GetComponent<Animator>();
        Anim.SetInteger("State", 0);

        body = GetComponent<Rigidbody>();        
	
	}
	
	// Update is called once per frame
	void Update () {
        //movimiento del jugador (esta OP!)
        if (moving)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Anim.SetInteger("State", 2);
                movex = -1;
                if (Input.GetKey(KeyCode.W))
                {
                    movey = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    movey = -1;
                }
                else
                    movey = 0;
                InMove();
            }

            else if (Input.GetKey(KeyCode.D))
            {
                Anim.SetInteger("State", 3);
                movex = 1;
                if (Input.GetKey(KeyCode.W))
                {
                    movey = 1;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    movey = -1;
                }
                else
                    movey = 0;
                InMove();
            }

            else if (Input.GetKey(KeyCode.W))
            {
                Anim.SetInteger("State", 4);
                movey = 1;
                if (Input.GetKey(KeyCode.D))
                {
                    movex = 1;
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    movex = 1;
                }
                else
                    movex = 0;
                InMove();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Anim.SetInteger("State", 1);
                movey = -1;
                if (Input.GetKey(KeyCode.D))
                {
                    movex = 1;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    movex = -1;
                }
                else
                    movex = 0;
                InMove();
            }
            else {
                movey = 0;
                movex = 0;
            }
        }
      //<------------------el movimiento acaba como aqui       
	
	}

    //UPDATE FIJO PORQUE PODEMOS!(ademas garantiza un buen manejo de frames por segundo y esas cosas de nerds)
    void FixedUpdate ()
    {
        body.velocity = new Vector3(movex * Speed, movey * Speed, 0);
        if(movex == 0 && movey == 0)
        {
            Anim.SetInteger("State", 0);
        }
    }

    //"MIENTRAS" el pj se mueve
    void InMove()
    {
        Speed = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            Speed = 2;

    }
}
