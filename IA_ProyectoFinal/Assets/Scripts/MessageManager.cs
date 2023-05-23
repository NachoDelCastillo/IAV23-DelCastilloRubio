using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using static NX.MessageManager;

namespace NX
{
    public class MessageManager : MonoBehaviour
    {
        static MessageManager instance;
        public static MessageManager GetInstance() { return instance; }

        private void Awake()
        { instance = this; }


        public enum MessagesName { fadeBlackLine }
        [Serializable]
        public struct TypeOfMessage
        {
            public MessagesName name;

            public float secondsOnScreen;
            public float transitionTime;
            public Image image;
            public TMP_Text text;

        }
        [SerializeField]
        public TypeOfMessage[] messages;


        public enum ColorName { blue, red}
        [Serializable]
        public struct TextColors
        {
            public ColorName name;

            public Color color;
        }
        [SerializeField]
        TextColors[] textColors;


        public void PlayMessage(string messageText, ColorName textColor, MessagesName fadeBlackLine)
        {
            Color c = Array.Find(textColors, col => col.name == textColor).color;
            TypeOfMessage m = Array.Find(messages, message => message.name == fadeBlackLine);

            StartCoroutine(PlayMessage(messageText, m, c));
        }

        IEnumerator PlayMessage(string messageText, TypeOfMessage message, Color textColor)
        {
            float secondsOnScreen = message.secondsOnScreen;
            float transitionTime = message.transitionTime;

            // Image
            Image image = message.image;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            message.image.DOFade(.65f, transitionTime);
            // Texto
            TMP_Text text = message.text;
            text.text = messageText;
            text.color = new Color(textColor.r, textColor.g, textColor.b, 0);
            message.text.DOFade(1, transitionTime);

            yield return new WaitForSeconds(secondsOnScreen);

            // Image
            message.image.DOFade(0, transitionTime);
            // Texto

            message.text.DOFade(0, transitionTime);

        }
    }
}
