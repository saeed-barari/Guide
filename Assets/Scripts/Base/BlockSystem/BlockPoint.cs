using System;
using System.Collections.Generic;
using System.Linq;
using Base.BlockSystem;
using Base.MapItemTypes;
using DefaultNamespace;
using Tools;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base
{
    [ExecuteAlways]
    public class BlockPoint : MonoBehaviour
    {
        public WayPoint[] wayPoints;
        
        [Baldr.ReadOnly]
        public BlockView currentView = null;
        public List<WayPoint> AvailableWayPoints => wayPoints.Where(way => way.ConnectedNode is null).ToList();
        public List<WayPoint> ConnectedWayPoints => wayPoints.Where(way => way.ConnectedNode is not null).ToList();

        public Vector3 TransformWayPoint(WayPoint wayPoint) => transform.TransformPoint(wayPoint.position);

        #region actions

        public event Action onDestroy;

        #endregion
        [Serializable] public class WayPoint
        {
            #region serializables
            [SerializeField, Baldr.ReadOnly] private string id = "";
            [Baldr.ReadOnly] public BlockPoint block;
            public Vector3 position;
            [Baldr.ReadOnly, SerializeField] public string connectedNodeId;
            public string ConnectedNodeId => connectedNodeId;
            #endregion

            public string ID => id;
            public WayPoint ConnectedNode
            {
                get => WaypointDataSaver.GetWaypoint(connectedNodeId);
                set
                {
                    connectedNodeId = value?.id ?? "";
                    block.UpdateView();
                }
            }

            public void Init()
            {
                id = Guid.NewGuid().ToString();
            }
        }

        public void Init()
        {
            foreach (var wayPoint in wayPoints) wayPoint.block = this;
            
            // setup waypoints
            foreach (var wayPoint in wayPoints) wayPoint.Init();
            BuildWaypointData();
        }

        public void BuildWaypointData()
        {
            wayPoints.Foreach(WaypointDataSaver.Add);
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
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
                Handles.color = w.ConnectedNode is not null ? Color.green : Color.red;
                Handles.DrawAAPolyLine(8, transform.position, TransformWayPoint(w));
                Handles.color = Color.green;
                Handles.DrawSolidDisc(TransformWayPoint(w), Vector3.up, 0.05f);
            }
        }
#endif
    }

}