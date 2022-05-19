using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base.MapItemTypes
{
    public class BlockView : MonoBehaviour
    {
        public Vector3[] waypoints;

        /// returns whether or not it can spawn for the specfied waypoints.
        /// if the answer is true, it also returns the float of which it has to rotate in order to face the waypoint
        public (bool canSpawn, float rotation) CanSpawnFor(Vector3[] waypoints)
        {
            if (waypoints.Length != this.waypoints.Length) return (false, 0);
            
            const int rotatingIncreament = 90;
            
            for (float angle = 0; angle < 360f; angle += rotatingIncreament)
            {
                for (int i = 0; i < this.waypoints.Length; i++)
                {
                    var rotatedWaypoint = Quaternion.Euler(0, angle, 0) * this.waypoints[i];
                    
                    // checking if any waypoint is close to them
                    if(waypoints.Any(wp => Vector3.Distance(wp, rotatedWaypoint) < 0.1) == false)
                        goto nextAngle;
                }
                
                // when reached here, it's apprived
                return (true, angle);

                nextAngle: ;
            }

            return (false, 0);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(waypoints is null) return;
            foreach (var pos in waypoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(pos + Vector3.up * 0.5f), 0.1f);
            }
        }
#endif
    }
}