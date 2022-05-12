using System;
using UnityEngine;

namespace Base.MapItemTypes
{
    [Serializable]
    public class Link
    {
        public Wall wall;
        public Vector3 position;
        public Vector3 positionInWorld => wall.transform.TransformPoint(position);
        public Quaternion rotationInWorld => Quaternion.LookRotation(position, Vector3.up);
        [SerializeReference, HideInInspector]
        public Link linked = null;

        public static void Connect(Link a, Link b)
        {
            a.linked = b;
            b.linked = a;
        }
    }
}