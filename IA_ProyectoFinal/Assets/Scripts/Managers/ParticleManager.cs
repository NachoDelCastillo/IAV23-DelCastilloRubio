using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace NX
{
    public class ParticleManager : MonoBehaviour
    {
        static ParticleManager instance;
        public static ParticleManager GetInstance() { return instance; }


        [Serializable]
        struct ParticleEffect
        {
            public string name;
            public GameObject gObj;
        }

        [SerializeField]
        ParticleEffect[] particleEffects;

        private void Awake()
        {
            instance = this;
        }


        public void Play(string particleName, Vector3 particlePosition)
        {
            Play(particleName, particlePosition, Vector3.zero);
        }

        public void Play(string particleName, Vector3 particlePosition, Vector3 size)
        {
            ParticleEffect p = Array.Find(particleEffects, effect => effect.name == particleName);

            if (p.gObj == null)
            {
                Debug.LogWarning("ParticleEffect: " + name + " not found!");
                return;
            }

            GameObject particleObj = Instantiate(p.gObj, particlePosition, Quaternion.identity, transform);
            if (size != Vector3.zero)
                particleObj.transform.localScale = size;
            Destroy(particleObj, 2f);
        }

    }
}
