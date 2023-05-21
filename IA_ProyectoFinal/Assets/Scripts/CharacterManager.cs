using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NX
{
    public class CharacterManager : MonoBehaviour
    {
        public Transform lockOnTransform;

        [Header("MovementFlags")]
        public bool isRotatingWithRootMotion;
    }
}
