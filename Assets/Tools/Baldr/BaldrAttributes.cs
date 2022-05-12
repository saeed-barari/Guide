using System;
using UnityEngine;

namespace BaldrAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public Color color = new Color(1, 1, 1, 1);
        public float height = 25;

        public ButtonAttribute(float r, float g, float b, float height)
        {
            this.color = new Color(r, g, b);
            this.height = height;
        }

        public ButtonAttribute(float r, float g, float b)
        {
            this.color = new Color(r, g, b);
        }

        public ButtonAttribute(float height)
        {
            this.height = height;
        }

        public ButtonAttribute() { }
    }

}