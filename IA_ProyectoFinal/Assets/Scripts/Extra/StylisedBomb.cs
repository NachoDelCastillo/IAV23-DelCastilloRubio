using NX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StylisedBomb : MonoBehaviour
{
    [SerializeField] private VisualEffect sparkParticles;

    private void Awake()
    {
        sparkParticles.Stop();
    }
}
