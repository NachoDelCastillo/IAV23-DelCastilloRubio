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




        #region VFX

        [SerializeField]
        TrailRenderer[] punchTr;

        [SerializeField]
        TrailRenderer[] footsTr;

        private void Start()
        {
            foreach (TrailRenderer trail in punchTr)
                trail.emitting = false;

            foreach (TrailRenderer trail in footsTr)
                trail.emitting = false;
        }

        public void EnableTrailOnPunch()
        {
            foreach (TrailRenderer trail in punchTr)
                trail.emitting = true;
        }

        public void DisableTrailOnPunch()
        {
            foreach (TrailRenderer trail in punchTr)
                trail.emitting = false;
        }


        public void EnableTrailOnFoot()
        {
            foreach (TrailRenderer trail in footsTr)
                trail.emitting = true;
        }

        public void DisableTrailOnFoot()
        {
            foreach (TrailRenderer trail in footsTr)
                trail.emitting = false;
        }

        #endregion
    }
}
