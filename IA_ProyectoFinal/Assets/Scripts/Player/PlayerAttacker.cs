using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NX
{
    public class PlayerAttacker : MonoBehaviour
    {
        public string lightAttack_Name;
        public string heavyAttack_Name;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        public void HandleLightAttack()
        {
            animatorHandler.PlayTargetAnimation(lightAttack_Name, true);
        }

        public void HandleHeavyAttack()
        {
            animatorHandler.PlayTargetAnimation(heavyAttack_Name, true);
        }
    }
}
