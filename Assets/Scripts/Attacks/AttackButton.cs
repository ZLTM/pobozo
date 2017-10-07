using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour {

    public BaseAttack magicAttackToPerform;

    public void CastMagicAttack()
    {
        GameObject.Find("BM").GetComponent<BattleState>().InPutMagic(magicAttackToPerform);
    }
}
