using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : EnemyController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        enemyName = "Witch";
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!attackMode)
            {
                Move();
            }

            if (!WithinBorders() && !inRange && !isAttacking())
            {
                SelectTarget();
            }

            if (inRange)
            {
                EnemyLogic();
            }
        }
    }
}
