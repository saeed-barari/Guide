using System;
using System.Collections.Generic;
using System.Linq;
using Baldr;
using Base.MapItemTypes;
using DefaultNamespace;
using Tools;
using UnityEngine;

namespace Base
{
    public class BlocksMap : MonoBehaviour
    {
        public List<BlockPoint> blockPoints = new List<BlockPoint>();

        [Button(1, 0, 0)]
        public void ResetPoints()
        {
            for (int i = 0; i < blockPoints.Count; i++)
            {
                if (Application.isPlaying)
                    Destroy(blockPoints[i].gameObject);
                else
                    DestroyImmediate(blockPoints[i].gameObject);
            }

            blockPoints.Clear();
            blockPoints.Add(Instantiate(SceneContext.Instance.blocksContainer.blockPointPrefabs, transform));
        }

        [Button]
        public void AddNewPoint()
        {
            var selectedPoint = blockPoints.Where(bp => bp.AvailableWayPoints.Count > 0).ToList().GetRandom();
            AddPointAt(selectedPoint, selectedPoint.AvailableWayPoints.GetRandom());
        }

        private void AddPointAt(BlockPoint blockPoint, Base.BlockPoint.WayPoint waypoint)
        {
            var newBlockPoint = Instantiate(SceneContext.Instance.blocksContainer.blockPointPrefabs, transform);
            newBlockPoint.transform.position = blockPoint.TransformWayPoint(waypoint) * 2 - blockPoint.transform.position;
            
            UpdateWayPointConnections(newBlockPoint);
            

            blockPoints.Add(newBlockPoint);
        }

        private void UpdateWayPointConnections(BlockPoint newBlockPoint)
        {
            foreach (var bp in blockPoints)
            {
                if(ReferenceEquals(bp, newBlockPoint))
                    continue;
                // if(Vector3.Distance(bp.transform.position, newBlockPoint.transform.position) > 3)
                //     continue;
                
                var availableWayPoints = newBlockPoint.AvailableWayPoints;
                
                foreach (var wp in bp.AvailableWayPoints)
                {
                    foreach (var newWp in availableWayPoints)
                    {
                        if (Vector3.Distance(newBlockPoint.TransformWayPoint(newWp), bp.TransformWayPoint(wp)) < 0.1f)
                        {
                            ConnectWayPoints(newWp, wp);
                            newBlockPoint.UpdateView();
                            bp.UpdateView();
                        }
                        
                    }
                }
            }
            
            void ConnectWayPoints(BlockPoint.WayPoint waypoint, BlockPoint.WayPoint wp)
            {
                wp.connectedNode = waypoint;
                waypoint.connectedNode = wp;
            }
        }

    }
}