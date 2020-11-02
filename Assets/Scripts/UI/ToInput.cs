using System;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class ToInput : MonoBehaviour
    {
        private InputField field;
        private Settings settings;
        void Awake()
        {
            field = GetComponent<InputField>();
            settings = GameObject.FindGameObjectWithTag("settings").GetComponent<Settings>();
            field.text = settings.maxOpponentsCount.ToString();
        }

        public void HandleInput(string input)
        {
            if (field.text == "") { return; }
            try
            {
                int intValue = Int32.Parse(field.text);
                settings.maxOpponentsCount = intValue;
            }
            catch (FormatException)
            {
                field.text = settings.maxOpponentsCount.ToString();
            }
        }
    }
}