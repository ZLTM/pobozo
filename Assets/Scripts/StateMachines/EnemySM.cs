using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySM : MonoBehaviour
{
    private bool isruning;
    private bool descon;

    public EnemyStats Enemy;//creamos a enemigo desde la plantilla
    private BattleState BS;
    public bool actionStarted = false;
    public GameObject Description;
    private HandleTurn myAttack;
    public string targetName;
    public enum enemyTurn
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public static enemyTurn currentEnemyTurn;
    private float curCooldown = 0f;
    public float maxCooldown = 2f;
    public Text descripttext;
    private bool wait;
    

    //para cambiar las barras
    private float curHealth;
    public GameObject HeroToAttack;

    //alive
    private bool alive = true;

    // Use this for initialization
    void Start()
    {
        Description.SetActive(false);
        myAttack = new HandleTurn();
        currentEnemyTurn = enemyTurn.PROCESSING;
        BS = GameObject.Find("BM").GetComponent<BattleState>();

    }
    // Update is called once per frame
    void Update()
    {

           
        if (GMScript.InBattle == true)
        {            
            curHealth = Enemy.CurrentHp;
            switch (currentEnemyTurn)
            {
                case (enemyTurn.PROCESSING):
                    UpgradeBar();
                    break;

                case (enemyTurn.CHOOSEACTION):
                    //print("enemy chooseaction");
                    ChooseAction();
                    currentEnemyTurn = enemyTurn.WAITING;

                    break;

                case (enemyTurn.WAITING):
                    //idle state                   
                    break;

                case (enemyTurn.ACTION):
                   // print("enemy action");
                    StartCoroutine(TimeForAction());

                    break;

                case (enemyTurn.DEAD):
                    if (!alive)
                    {
                        return;
                    }
                    else
                    {
                        //change tag
                        this.gameObject.tag = "DeadEnemy";
                        //remove from list
                        BS.EnemiesInBattle.Remove(this.gameObject);
                        //quitar atackes
                        if (BS.EnemiesInBattle.Count > 0)
                        {
                            for (int i = 0; i < BS.PerformList.Count; i++)
                            {
                                if (BS.PerformList[i].AttackerGO == this.gameObject)
                                {
                                    BS.PerformList.Remove(BS.PerformList[i]);
                                }
                                if (BS.PerformList[i].TargetGO == this.gameObject)
                                {
                                    BS.PerformList[i].TargetGO = BS.EnemiesInBattle[Random.Range(0, BS.EnemiesInBattle.Count)];
                                }
                            }
                        }

                        //set alive false
                        alive = false;
                        //revisamos los botones
                        BS.CreateEnemyButtons();
                        //revisamos estado
                        BS.battleStates = BattleState.performAction.CHECKALIVE;
                    }
                    break;
            }
        }
    }



    void ChooseAction()
    {
        myAttack = new HandleTurn();
        myAttack.Attacker = Enemy.baseName;
        myAttack.Side = "Enemy";
        myAttack.AttackerGO = this.gameObject;
        myAttack.TargetGO = BS.HeroInGame[Random.Range(0, BS.HeroInGame.Count)];
        HeroToAttack = myAttack.TargetGO;
        int num = Random.Range(0, Enemy.Attacks.Count);//se genra un numero aleatorio entre 0 y la cantidad de ataques
        myAttack.chooseAttack = Enemy.Attacks[num];//usamos el numero para escoger el ataque
        if (!isruning)
        {
            StartCoroutine(waitsec());
        }
         Debug.Log(this.gameObject.name + " has choosen " + myAttack.chooseAttack.attackName + " and did " + myAttack.chooseAttack.attackDamage + " damage!!!");
        BS.CollectActions(myAttack);
    }

    IEnumerator waitsec()
    {   
        isruning = true;  
        print("true now");
        if (Description.activeSelf == false)
        {
            print("true dat");
            Description.SetActive(true);
            descripttext.text = this.gameObject.name + " ha usado " + myAttack.chooseAttack.attackName + " y te ha hecho " + myAttack.chooseAttack.attackDamage + " puntos de daño!!!";
            yield return new WaitForSeconds(1);
            isruning = false;
        }
        else if (Description.activeSelf == true)
        {
            Description.SetActive(false);
            print("escription");
            yield return null;
            isruning = false;
        }

        

    }
   /* IEnumerator waitfalse()
    {
        Description.SetActive(false);

        yield return new WaitForSeconds(waits);

    }*/


    private IEnumerator TimeForAction()//controla variables atraves del tiempo
    {
        
        if (actionStarted == true)
        {
            yield break;
        }
        actionStarted = true;
        //Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x+1.5f, HeroToAttack.transform.position.y + 1.5f, HeroToAttack.transform.position.z + 1.5f);

        //wait
        //do damage
        
        //if (HeroToAttack.GetComponent<HeroSM>() != null)
        //{
            DoDamage();
        //}
            //remove from bs list
        BS.PerformList.RemoveAt(0);//remueve al objeto en 0, el siguiente sube transformando el 1 en 0
                                   //continua removiendo hasta desaparecer todo
                                   //reset bs to wait
        BS.battleStates = BattleState.performAction.WAIT;
        //al pasar a wait y saltar a takeaction los objetos vuelven a crearse repitiendo el ciclo
        //end coroutine
        actionStarted = false;
        currentEnemyTurn = enemyTurn.PROCESSING;
    }
    void DoDamage()
    {
        float calcDamage = Enemy.Fue * BS.PerformList[0].chooseAttack.attackDamage;
        targetName = HeroToAttack.name;
        if (targetName == "Juan")
        {
            GameObject.Find("JuanPan").GetComponent<HeroSM>().TakeDamage(calcDamage);
        }
        else if (targetName == "Ana")
        {
            GameObject.Find("AnaPan").GetComponent<HeroSM>().TakeDamage(calcDamage);
        }
    }

    public void TakeDamage(float getDamageAmount)
    {
        Enemy.CurrentHp -= getDamageAmount;
        if(Enemy.CurrentHp <= 0)
        {
            Enemy.CurrentHp = 0;
            currentEnemyTurn = enemyTurn.DEAD;
        }
    }

    void UpgradeBar()
    {
        curCooldown = curCooldown + Time.deltaTime;
        if(curCooldown >= maxCooldown)
        {
            currentEnemyTurn = enemyTurn.CHOOSEACTION;
            curCooldown = 0f;
        }
    }

}