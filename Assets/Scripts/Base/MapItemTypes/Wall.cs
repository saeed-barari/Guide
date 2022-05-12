using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaldrAttributes;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base.MapItemTypes
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Link[] links;
        [SerializeField] private Collider coveringArea;

        public List<Link> GetOpenLinks() => (from link in links where link.linked is null select link).ToList();
        public bool WillCollideWith(Wall wall) => coveringArea.bounds.Intersects(wall.coveringArea.bounds);
        

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(links is null) return;
            foreach (var link in links)
            {
                if(link.wall is null) continue;
                Handles.color = Color.black;
                Handles.DrawAAPolyLine(8f, link.wall.transform.position, link.positionInWorld);
                Handles.color = Color.red;
                Handles.DrawSolidDisc(link.positionInWorld, Vector3.up, 0.1f);
                
                if(link.linked is null) continue;
                Handles.color = Color.white;
                Handles.DrawAAPolyLine(8f, link.positionInWorld, link.linked.positionInWorld);
            }
        }

        [Button(0, 1, 0)]
        private void AddWall()
        {
            var tmp = links?.ToList() ?? new List<Link>();
            tmp.Add(new Link() {wall = this});
            links = tmp.ToArray();
        }
#endif
        /// creates a new wall and aligns it's relevant link to the old link. and automatically resolves it's transform
        public static Wall MakeNewWallAndAlignToLink(Link oldLink, Link newLink)
        {
            var newLinkIndex = Array.FindIndex(newLink.wall.links, x => x == newLink);
            var newWall = Instantiate(newLink.wall);
            var newWallLink = newWall.links[newLinkIndex];
            Link.Connect(newWallLink, oldLink); // make the links point ot each other

            // resolving position
            newWall.transform.position = oldLink.positionInWorld * 2 - oldLink.wall.transform.position;
            
            // resolving rotation
            newWall.transform.rotation.SetFromToRotation(newWallLink.positionInWorld, oldLink.positionInWorld);
            return newWall;
        }

    }
}