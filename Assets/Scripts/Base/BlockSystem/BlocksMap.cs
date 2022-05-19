using System;
using System.Collections.Generic;
using System.Linq;
using Baldr;
using Base.BlockSystem;
using Base.MapItemTypes;
using DefaultNamespace;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base
{
    public class BlocksMap : MonoBehaviour
    {
        public List<BlockPoint> blockPoints = new List<BlockPoint>();

        public BaseAutoSetConfig config;

        #region passed block variables
        private List<BlockPoint> _passedBlockPoints = new List<BlockPoint>();
        private const float PASSED_BLOCK_CHECK_RATE = 0.5f;
        private const float PASSED_BLOCK_CHECK_RANGE = 20f;
        private float last_passed_block_check = 0;
        #endregion

        private float lastUpdateTime = 0;

        private void Start()
        {
            BuildBlockWaypointsData();
        }

        private void Update()
        {
            if (Time.time - lastUpdateTime > config.reSpawnTime)
            {
                lastUpdateTime = Time.time;
                if(RemoveFarPassedBlocks() == true)
                    SpawnNewBlock();
            }
            if(Time.time - - last_passed_block_check > PASSED_BLOCK_CHECK_RATE)
            {
                last_passed_block_check = Time.time;
                CheckPassedBlocks();
            }
        }

        private void CheckPassedBlocks()
        {
            foreach (var blockPoint in blockPoints)
            {
                if (_passedBlockPoints.Contains(blockPoint)) continue;
                if(Vector3.Distance(SceneContext.Instance.playerController.transform.position, blockPoint.transform.position) < PASSED_BLOCK_CHECK_RANGE)
                    _passedBlockPoints.Add(blockPoint);
            }
        }

        private bool RemoveFarPassedBlocks()
        {
            for (int i = 0; i < _passedBlockPoints.Count; i++)
            {
                if(Vector3.Distance(_passedBlockPoints[i].transform.position, SceneContext.Instance.playerController.transform.position) > config.maxDistance)
                {
                    var bp = _passedBlockPoints[i];
                    Debug.Log($"removing {bp.name}");
                    
                    bp.ConnectedWayPoints
                        .Foreach(wp =>
                        {
                            var neighborNode = wp.ConnectedNode!;
                            neighborNode.ConnectedNode = null; // removing connection
                        });

                    // remove
                    blockPoints.Remove(bp);
                    _passedBlockPoints.RemoveAt(i);
                    Destroy(bp.gameObject);

                    return true;
                }
            }
            return false;
        }

        private void SpawnNewBlock()
        {
            var possibleBlocks = blockPoints.Where(bp => !_passedBlockPoints.Contains(bp)).ToList();
            if (possibleBlocks.Count == 0)
            {
                possibleBlocks = blockPoints;
            }
            var block = possibleBlocks.GetRandom();
            
            if(block.AvailableWayPoints.Count == 0) return;
            Debug.Log($"adding point");
            AddPointAt(block, block.AvailableWayPoints.GetRandom());
        }


        [Button]
        void printWaypointDataSaver()
        {
            Debug.Log("Count " + WaypointDataSaver.wayPoints.Count + "\n" + string.Join("\n",
                WaypointDataSaver.wayPoints.Select((wp, i) => i + ": " + wp.ID)));
        }
        [Button]
        public void BuildBlockWaypointsData()
        {
            blockPoints.Foreach(block => block.BuildWaypointData());
        }
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
            var blockPoint = Instantiate(SceneContext.Instance.blocksContainer.blockPointPrefabs, transform);
            blockPoint.Init();
            blockPoints.Add(blockPoint);
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
            newBlockPoint.Init();
            newBlockPoint.name += " " + blockPoints.Count;
            newBlockPoint.transform.position = blockPoint.TransformWayPoint(waypoint) * 2 - blockPoint.transform.position;
            
            UpdateWayPointConnections(newBlockPoint);
            

            blockPoints.Add(newBlockPoint);
        }

        private void UpdateWayPointConnections(BlockPoint newBlockPoint)
        {
            foreach (var bp in blockPoints)
            {
                if(bp == newBlockPoint) continue;
                
                var availableWayPoints = newBlockPoint.AvailableWayPoints;
                
                foreach (var wp in bp.AvailableWayPoints)
                {
                    foreach (var newWp in availableWayPoints)
                    {
                        if (Vector3.Distance(newBlockPoint.TransformWayPoint(newWp), bp.TransformWayPoint(wp)) < 0.1f)
                        {
                            ConnectWayPoints(newWp, wp);
                            newBlockPoint.UpdateView();
                        }
                        
                    }
                }
            }
            
            void ConnectWayPoints(BlockPoint.WayPoint waypoint, BlockPoint.WayPoint wp)
            {
                wp.ConnectedNode = waypoint;
                waypoint.ConnectedNode = wp;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(config is null) return;
            Gizmos.color = new Color(1, 0, 1, 0.2f);
            Gizmos.DrawSphere((Vector3) SceneContext.Instance?.playerController?.transform.position, config.maxDistance);
        }
#endif
    }
}