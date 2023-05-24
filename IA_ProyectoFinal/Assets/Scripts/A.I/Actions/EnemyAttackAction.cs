using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NX
{
    [CreateAssetMenu(menuName = "A.I/EnemyActions/AttackAction")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximunAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}
