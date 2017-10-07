using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//SIGUIENTE PASO     continuar tutorial revisado, revisar null reference en DoDamage ¿add list de heroes?
public class HeroSM : MonoBehaviour
{
    public AudioSource attsound;
    
    public AudioClip useclip;

    private BattleState BS;
    public static bool inBattle;
    public partyStat hero;//creamos a hero desde la plantilla

    public enum heroTurn
    {
        START,
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public static heroTurn currentHeroTurn;
    //para cambiar las barras
    private float curHealth;
    public Image healthB;
    public Image chamaB;
    public GameObject selector;
    public GameObject EnemyToAttack;
    public GameObject AttackPanel;
    //ienumerator
    private float curCooldown = 0f;
    private float maxCooldown = 5f;
    public Image progressBar;
    //dead
    private bool alive = true;

    private bool actionStarted;

    public float magicDam;
    // Use this for initialization
    void Start()
    {
        maxCooldown = Random.Range(0, maxCooldown);
        selector.SetActive(false);
        BS = GameObject.Find("BM").GetComponent<BattleState>();
        currentHeroTurn = heroTurn.START;

    }

    // Update is called once per frame
    void Update()
    {
        
        /*if (EnemySM.currentEnemyTurn != EnemySM.enemyTurn.WAITING)
        {
            currentHeroTurn = heroTurn.WAITING;
        }*/

        //creamos el movimiento de las barras de hp y chama
        chamaB.transform.localScale = new Vector3(hero.CurrentChama / 100, this.transform.localScale.y, this.transform.localScale.z);
        healthB.transform.localScale = new Vector3(hero.CurrentHp / 100, this.transform.localScale.y, this.transform.localScale.z);
        curHealth = hero.CurrentHp;
        if (GMScript.InBattle == true)
        {
            switch (currentHeroTurn)
            {
                case (heroTurn.START):

                    currentHeroTurn = heroTurn.PROCESSING;
                    break;
                case (heroTurn.PROCESSING):
                    //inBattle = true;
                    //print("hero processing");
                    UpgradeBar();
                    break;
                case (heroTurn.ADDTOLIST):
                    //print("hero addtolist");
                    BS.HeroesToManage.Add(this.gameObject);
                    currentHeroTurn = heroTurn.WAITING;
                    break;
                case (heroTurn.WAITING):
                    //print("hero waiting");
                    /* if (EnemySM.currentEnemyTurn == EnemySM.enemyTurn.WAITING)
                     {
                         currentHeroTurn = heroTurn.START;
                     }*/
                    //idle
                    break;
                case (heroTurn.ACTION):
                    //print("hero action");
                    StartCoroutine(TimeForAction());
                    break;
                case (heroTurn.DEAD):
                    BS.battleStates = BattleState.performAction.LOOSE;
                    //if para implementar revivir
                    if (!alive)
                    {
                        return;
                    }
                    else
                    {
                        BS.battleStates = BattleState.performAction.LOOSE;
                        //change tag
                        this.gameObject.tag = "DeadHero";
                        //not attackable
                        BS.HeroInGame.Remove(this.gameObject);//????
                        //not manageable
                        BS.HeroesToManage.Remove(this.gameObject);
                        //deactivate selector
                        selector.SetActive(false);
                        //reset gui
                        BS.AttackPanel.SetActive(false);
                        BS.EnemySelectPanel.SetActive(false);
                        BS.StatusPanel.SetActive(false);
                        //remove item from performlist
                        if (BS.HeroInGame.Count > 0)
                        {
                            for (int i = 0; i < BS.PerformList.Count; i++)
                            {
                                if (BS.PerformList[i].AttackerGO = this.gameObject)
                                {
                                    BS.PerformList.Remove(BS.PerformList[i]);
                                }

                                if (BS.PerformList[i].TargetGO == this.gameObject)
                                {
                                    BS.PerformList[i].TargetGO = BS.HeroInGame[Random.Range(0, BS.HeroInGame.Count)];
                                }
                            }
                        }
                        //reactivar si es necesario
                        /*
                        if (BS.PerformList.Count < 1)
                        {
                            BS.AttackPanel.SetActive(false);
                            BS.EnemySelectPanel.SetActive(false);
                            BS.StatusPanel.SetActive(false);
                            //GMScript.isDead = true;
                        }*/
                        //¿change color?
                        //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105,105,105,255);
                        //reset heroinput
                        BS.battleStates = BattleState.performAction.CHECKALIVE;
                        alive = false;
                    }
                    //GMScript.isDead = true;

                    break;
            }
        }
    }
    void UpgradeBar()
    {
        curCooldown = curCooldown + Time.deltaTime;
        float calcCooldown = curCooldown / maxCooldown;
        progressBar.transform.localScale = new Vector3(Mathf.Clamp(calcCooldown, 0, 1), progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        if (curCooldown >= maxCooldown)
        {
            AttackPanel.SetActive(true);
            currentHeroTurn = heroTurn.ADDTOLIST;
            curCooldown = 0f;
        }

        
    }
    private IEnumerator TimeForAction()//controla variables atraves del tiempo
    {
        if (actionStarted == true)
        {
            yield break;
        }
        actionStarted = true;
        //wait
        yield return new WaitForSeconds(0.5f);
        //do damage
        magicDam = BattleState.dam;
        DoDamage(magicDam);
        //remove from bs list
        BS.PerformList.RemoveAt(0);//remueve al objeto en 0, el siguiente sube transformando el 1 en 0
                                   //continua removiendo hasta desaparecer todo
                                   //reset bs to wait
        if(BS.battleStates != BattleState.performAction.WIN && BS.battleStates != BattleState.performAction.LOOSE)
        {
            BS.battleStates = BattleState.performAction.WAIT;
            curCooldown = 0f;
            currentHeroTurn = heroTurn.PROCESSING;
        }
        else
        {
            currentHeroTurn = heroTurn.WAITING;
        }
        //al pasar a wait y saltar a takeaction los objetos vuelven a crearse repitiendo el ciclo
        //end coroutine

        actionStarted = false;
    }
    public void TakeDamage(float getDamageAmount)
    {
        hero.CurrentHp -= getDamageAmount;
        if (hero.CurrentHp <= 0)
        {
            hero.CurrentHp = 0;
            currentHeroTurn = heroTurn.DEAD;
            BS.battleStates = BattleState.performAction.LOOSE;

        }
    }

    void DoDamage (float magicDmg)//usamos magicDmg como testeo en debug para implementar cambios en daños magicos
    {
        attsound.Play();       
        float calcDamage = hero.Fue + BS.PerformList[0].chooseAttack.attackDamage;//error en attackDamage
        EnemyToAttack.GetComponent<EnemySM>().TakeDamage(calcDamage);
    }
}