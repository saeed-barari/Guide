using System;
using System.Collections;
using System.Collections.Generic;
using BaldrAttributes;
using Base;
using Base.MapItemTypes;
using DefaultNamespace;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    /// returns the possible wall builds from the given walls, On the given index
    public PossibleBuildResult GetPossibleWallBuilds(List<Wall> walls, int index)
    {
        var results = new PossibleBuildResult();
        
        var currentWall = walls[index];

        var openLinks = currentWall.GetOpenLinks();
        
        foreach (var openLink in openLinks)
        {
            foreach (var otherWall in SceneContext.Instance.wallsContainer.walls)
            {
                if (currentWall.WillCollideWith(otherWall)) continue;
                
                // add all links
                foreach (var otherLink in otherWall.GetOpenLinks())
                {
                    results.buildableWalls.Add(new PossibleBuildResult.BuildableWall
                    {
                        from = openLink,
                        to = otherLink
                    });
                }
            }
        }

        return results;
    } 
    
    [Serializable]
    public class PossibleBuildResult
    {
        public List<BuildableWall> buildableWalls = new List<BuildableWall>();
        [Serializable]
        public struct BuildableWall
        {
            public Link from;
            public Link to;
        }

        [Button]
        public void Hey()
        {
            
        }
    }
}

