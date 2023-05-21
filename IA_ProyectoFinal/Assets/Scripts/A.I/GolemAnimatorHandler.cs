using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public class GolemAnimatorHandler : EnemyAnimatorHandler
    {
        [SerializeField]
        DamageCollider swing;

        [SerializeField]
        DamageCollider floorSlam;

        [SerializeField]
        GameObject explosion;


        public void EnableDC_Swing()
        { swing.EnableDamageCollider(); }
        public void DisableDC_Swing()
        { swing.DisableDamageCollider(); }

        public void EnableDC_FloorSlam()
        { floorSlam.EnableDamageCollider(); }
        public void DisableDC_FloorSlam()
        { floorSlam.DisableDamageCollider(); }

        public void ExplosionFVX()
        {
            GameObject newExplosion = Instantiate(explosion, transform);

            newExplosion.transform.localScale = Vector3.one * 2.15f;

            Destroy(newExplosion, 0.5f);
        }
    }
}
