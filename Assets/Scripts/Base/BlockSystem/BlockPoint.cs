#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using BaldrAttributes;
using Base.MapItemTypes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base
{
    public class BlockPoint : MonoBehaviour
    {
        public WayPoint[] wayPoints;
        
        [ReadOnly]
        public BlockView currentView = null;
        public List<WayPoint> availableWayPoints => wayPoints.Where(way => way.connectedNode is null).ToList();

        public Vector3 TransformWayPoint(WayPoint wayPoint) => transform.TransformPoint(wayPoint.position);
            
        [Serializable] public class WayPoint
        {
            public Vector3 position;
            [NonSerialized] public WayPoint? connectedNode;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var w in wayPoints)
            {
                Handles.color = w.connectedNode is not null ? Color.green : Color.red;
                Handles.DrawAAPolyLine(8, transform.position, TransformWayPoint(w));
                Handles.color = Color.green;
                Handles.DrawSolidDisc(TransformWayPoint(w), Vector3.up, 0.05f);
            }
        }
#endif
    }

}