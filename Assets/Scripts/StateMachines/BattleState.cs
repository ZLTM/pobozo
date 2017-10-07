using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleState : MonoBehaviour
{

    public AudioClip magclip;
    public AudioClip attclip;

    public MovieTexture Ending;
    public AudioSource aud;
    public enum performAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOOSE
    }
    public performAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();//lista de actores
    public List<GameObject> HeroInGame = new List<GameObject>();//heroes en la escena
    public List<GameObject> EnemiesInBattle = new List<GameObject>();//enemigos en la escena

    public enum HeroGUi//acciones posibles del heroe
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }
    public HeroGUi HeroInput;

    public List<GameObject> HeroesToManage = new List<GameObject>();
    private HandleTurn HeroChoice;
    public GameObject enemyButton;
    public Transform Spacer;
    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject StatusPanel;
    public GameObject Defeat;
    public static float dam;
    public GameObject master;

    public GameObject MagicPanel;

    //magic attack
    //public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButtonInSC;
    public GameObject magicButtonInSC;

    public AudioSource mback;
    public AudioSource bback;

    private List<GameObject> atkBtns = new List<GameObject>();

    //enemy buttons
    private List<GameObject> enemyBtn = new List<GameObject>();
    
    void Start()
    {
        Defeat.SetActive(false);
        battleStates = performAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroInGame.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUi.ACTIVATE;
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        StatusPanel.SetActive(false);
        MagicPanel.SetActive(false);

        CreateEnemyButtons();

    }
    // Update is called once per frame
    void Update()
    {
        if (Ending != null)
        {
            if (Ending.isPlaying == true)
            {
                print("muting");
                mback.mute = true;
                bback.mute = true;
            }

            if (Ending.isPlaying == false)
            {
                print("not muting");
                mback.mute = false;
                bback.mute = false;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Screen.fullScreen = false;
                Ending.Stop();
            }
        }

        if (HeroSM.inBattle == true)
        {
            StatusPanel.SetActive(true);
            switch (battleStates)
            {
                case (performAction.WAIT):
                    if (PerformList.Count > 0)//revisa la lista
                    {
                        battleStates = performAction.TAKEACTION;
                    }
                    break;

                case (performAction.TAKEACTION):
                    GameObject performer = GameObject.Find(PerformList[0].Attacker);
                    if (PerformList[0].Side == "Enemy")
                    {
                        EnemySM ESM = performer.GetComponent<EnemySM>();
                        for(int i = 0; i<HeroInGame.Count; i++)
                        {
                            if (PerformList[0].TargetGO == HeroInGame[i])
                            {
                                ESM.HeroToAttack = PerformList[0].TargetGO;
                                EnemySM.currentEnemyTurn = EnemySM.enemyTurn.ACTION;
                                break;
                            }
                            else
                            {
                                PerformList[0].TargetGO = HeroInGame[Random.Range(0, HeroInGame.Count)];
                                ESM.HeroToAttack = PerformList[0].TargetGO;
                                EnemySM.currentEnemyTurn = EnemySM.enemyTurn.ACTION;
                            }
                        }
                        
                    }
                    if (PerformList[0].Side == "Hero")
                    {

                        HeroSM HSM = performer.GetComponent<HeroSM>();
                        HSM.EnemyToAttack = PerformList[0].TargetGO;
                        HeroSM.currentHeroTurn = HeroSM.heroTurn.ACTION;
                    }
                    battleStates = performAction.PERFORMACTION;
                    break;

                case (performAction.PERFORMACTION):
                    //iddle
                    break;

                case (performAction.CHECKALIVE):
                    if(HeroInGame.Count <= 1)
                    {
                        battleStates = performAction.LOOSE;
                        //losegame
                    }
                    else if (EnemiesInBattle.Count < 1)
                    {
                        battleStates = performAction.WIN;
                        //win battle
                    }
                    else
                    {
                        //limpiamos las ventanas
                        clearAttackPanel();
                        HeroInput = HeroGUi.ACTIVATE;
                    }
                break;

                case (performAction.WIN):
                    for(int i = 0; i < HeroInGame.Count; i++)
                    {
                        print("win");
                        aud = GetComponent<AudioSource>();
                        
                        aud.clip = Ending.audioClip;
                        Ending.Play();
                        aud.Play();
                        HeroSM.currentHeroTurn = HeroSM.heroTurn.WAITING;
                        StartCoroutine(IllBeBack());
                        

                    }
                break;

                case (performAction.LOOSE):
                    print("lose bs");
                    Defeat.SetActive(true);
                    break;
            }
            //switch para el estado del heroe

            switch (HeroInput)
            {
                case (HeroGUi.ACTIVATE):
                    if (HeroesToManage.Count > 0)
                    {
                        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);//tomamos al primer heroe de la lista, hallamos el padre, pasamos al hijo llamado selector y seteamos el booleano
                        HeroChoice = new HandleTurn();
                        AttackPanel.SetActive(true);
                        CreateAttackButtons();

                        HeroInput = HeroGUi.WAITING;
                    }
                    break;
                case (HeroGUi.WAITING):
                    //iddle
                    break;
                case (HeroGUi.DONE):
                    HeroInputDone();
                    break;
            }
        }
    }

    public void CollectActions(HandleTurn handleInput)
    {
        PerformList.Add(handleInput);
    }

    public void CreateEnemyButtons()
    {
        //limpiamos los botones
        foreach(GameObject enemyBut in enemyBtn)
        {
            Destroy(enemyBut);//<------lol
        }
        //creamos botones
        foreach (GameObject Enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();
            EnemySM curEnemy = Enemy.GetComponent<EnemySM>();
            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = curEnemy.Enemy.baseName;

            button.EnemyPrefab = Enemy;

            newButton.transform.SetParent(Spacer, false);
            enemyBtn.Add(newButton);
        }
    }
    public void Input1()//attack button
    {
        HeroesToManage[0].GetComponent<HeroSM>().attsound.clip = attclip;
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackerGO = HeroesToManage[0]; // ya es un gameobject asi que no lo especificamos
        HeroChoice.Side = "Hero";
        HeroChoice.chooseAttack = HeroesToManage[0].GetComponent<HeroSM>().hero.Attacks[0];
        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }
    public void InputEnemySelection(GameObject choosenEnemy)//enemy selection
    {
        HeroChoice.TargetGO = choosenEnemy;
        HeroInput = HeroGUi.DONE;
    }
    public void InputHability ()//muestra habilidades magicas input3
    {
        HeroesToManage[0].GetComponent<HeroSM>().attsound.clip = magclip;
        AttackPanel.SetActive(false);
        if (EnemySelectPanel.active == false)
        {
            MagicPanel.SetActive(true);
        }
    }
    void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        
        //clean panel
        clearAttackPanel();

        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGUi.ACTIVATE;
    }

    void clearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(false);
        //otros paneles?
        foreach (GameObject atkbt in atkBtns)
        {
            Destroy(atkbt);
        }
        atkBtns.Clear();

    }


    //creando botones de accion
    void CreateAttackButtons()
    {
        //creamos magia
        GameObject MagicButton = Instantiate(magicButtonInSC) as GameObject;
        Text MagicAttackButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";

        MagicButton.GetComponent<Button>().onClick.AddListener(() => InputHability());
        //diferent stuff
        MagicButton.transform.SetParent(magicSpacer, false);
        atkBtns.Add(MagicButton);

        if(HeroesToManage[0].GetComponent<HeroSM>().hero.MagicAttacks.Count >0)
        {
            foreach(BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroSM>().hero.MagicAttacks)
            {
                GameObject MagicBtn = Instantiate(magicButtonInSC) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicBtn.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                atkBtns.Add(MagicButton);
            }
        }

        else
        {
            MagicButton.GetComponent<Button>().interactable = false;
        }
    }

   public void InPutMagic(BaseAttack chooseMagic)//choose magic attack input4
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackerGO = HeroesToManage[0]; // ya es un gameobject asi que no lo especificamos
        HeroChoice.Side = "Hero";

        HeroChoice.chooseAttack = HeroesToManage[0].GetComponent<HeroSM>().hero.MagicAttacks[0];
        dam = HeroesToManage[0].GetComponent<HeroSM>().hero.MagicAttacks[0].attackDamage;
        //print(chooseMagic.attackCost);
        HeroesToManage[0].GetComponent<HeroSM>().useclip = magclip;

        HeroesToManage[0].GetComponent<HeroSM>().hero.CurrentChama -= HeroChoice.chooseAttack.attackCost;
        if(HeroesToManage[0].GetComponent<HeroSM>().hero.CurrentChama <= 0)
        {
            HeroesToManage[0].GetComponent<HeroSM>().hero.CurrentChama = 0;
        }
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
        
    }

    void OnGUI()
    {
        if (Ending != null && Ending.isPlaying)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Ending, ScaleMode.StretchToFill);
        }
    }
    IEnumerator IllBeBack ()
    {
        mback.mute = true;
        bback.mute = true;
        yield return new WaitForSeconds(6);
        Application.LoadLevel("StartMenu");

    }

}
 