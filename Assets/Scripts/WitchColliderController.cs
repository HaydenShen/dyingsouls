using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchColliderController : MonoBehaviour
{
    EnemyController enemyController;
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        enemyController = GetComponentInParent<EnemyController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Attack" && !enemyController.isDead)
        {
            enemyController.TakeDamage();
        }
    }
}
