#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Baldr;
using Base.MapItemTypes;
using DefaultNamespace;
using TMPro;
using Unity.Mathematics;
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
        public List<WayPoint> AvailableWayPoints => wayPoints.Where(way => way.connectedNode is null).ToList();
        public List<WayPoint> ConnectedWayPoints => wayPoints.Where(way => way.connectedNode is not null).ToList();

        public Vector3 TransformWayPoint(WayPoint wayPoint) => transform.TransformPoint(wayPoint.position);
            
        [Serializable] public class WayPoint
        {
            public Vector3 position;
            [NonSerialized] public WayPoint? connectedNode;
        }

        public void UpdateView()
        {
            var wayPointPoses = ConnectedWayPoints.Select(_ => _.position).ToArray();
            
            var (view, rotation) = SceneContext.Instance.blocksContainer.GetTheAppropriateBlockView(wayPointPoses);
            
            if (view is null) Debug.LogWarning($"no view for {string.Join(", ", wayPointPoses)}");
            else
            {
                if (currentView != null)
                {
#if UNITY_EDITOR
                    if(!Application.isPlaying)
                        DestroyImmediate(currentView.gameObject);
                    else
#endif
                    Destroy(currentView.gameObject);
                }

                currentView = Instantiate(view, transform);
                currentView.transform.localPosition = Vector3.zero;
                currentView.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            }
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