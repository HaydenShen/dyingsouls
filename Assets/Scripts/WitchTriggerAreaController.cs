using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchTriggerAreaController : MonoBehaviour
{
    private EnemyController enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            enemyParent.target = collider.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
