using UnityEngine;
using System.Collections;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("BM").GetComponent<BattleState>().InputEnemySelection(EnemyPrefab);// pasara el prefab
    }
}
