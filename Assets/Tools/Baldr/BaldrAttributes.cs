using System;
using UnityEngine;

namespace BaldrAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public Color color = new Color(1, 1, 1, 1);
        public string groupName = "Buttons";
        public float height = 27;

        public ButtonAttribute(string groupName, float r, float g, float b, float height = 27)
        {
            this.color = new Color(r, g, b);
            this.groupName = groupName;
            this.height = height;
        }
        public ButtonAttribute(float r, float g, float b, float height = 27)
        {
            this.color = new Color(r, g, b);
            this.height = height;
        }

        public ButtonAttribute(string groupName, float height)
        {
            this.groupName = groupName;
            this.height = height;
        }
        public ButtonAttribute(float height)
        {
            this.groupName = groupName;
            this.height = height;
        }

        public ButtonAttribute(string groupName)
        {
            this.groupName = groupName;
        }

        public ButtonAttribute() { }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class GroupAttribute : PropertyAttribute
    {
        public string label = "";
        public GroupAttribute(string label)
        {
            this.label = label;
        }
    }

}