using UnityEngine;
using System.Collections;

[System.Serializable]
public class HandleTurn
{
    public string Attacker;//name of the culprit
    public string Side;//enemigo o heroe
    public GameObject AttackerGO;//object of the culprit
    public GameObject TargetGO;//victim GO

    //ataque realisado
    public BaseAttack chooseAttack;
}
