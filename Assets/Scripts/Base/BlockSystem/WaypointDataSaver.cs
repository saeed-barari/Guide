using System.Collections.Generic;
using System.Linq;
using Baldr;
using UnityEngine;

namespace Base.BlockSystem
{
    public static class WaypointDataSaver 
    {
        public static List<BlockPoint.WayPoint> wayPoints { get; private set; } = new();

        public static void Add(BlockPoint.WayPoint wayPoint)
        {
            if(wayPoints.Contains(wayPoint)) return;
            
            wayPoints.Add(wayPoint);
            wayPoint.block.onDestroy += () =>
            {
                Debug.Log($"destroyed {wayPoint.ID}");
                wayPoints.Remove(wayPoint);
            };
        }
        public static BlockPoint.WayPoint GetWaypoint(string id) => wayPoints.FirstOrDefault(x => x.ID == id);
        
    }
}