using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Base.MapItemTypes;
using DefaultNamespace;
using BaldrAttributes;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Base
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Transform wallsParent;
        public List<Wall> walls = new List<Wall>();
        
        [SerializeField]
        private MapBuilder.PossibleBuildResult _result;


        private void Start()
        {
            ResetWalls();
        }

        [Button]
        public void GetPossibleWallBuildsForLastWall()
        {
            _result = SceneContext.Instance.mapBuilder.GetPossibleWallBuilds(walls, walls.Count - 1);
        }

        [Button(1, 0, 0)]
        public void ResetWalls()
        {
            for (int i = walls.Count - 1; i >= 0; i--)
            {
                if(Application.isPlaying) Destroy(walls[i]?.gameObject);
                else DestroyImmediate(walls[i]?.gameObject);
                walls.RemoveAt(i);
            }
            
            // create first wall at center
            walls.Add(Instantiate(SceneContext.Instance.wallsContainer.walls[0], Vector3.zero, quaternion.identity, wallsParent));
        }

        public int buildindex = 0;
        [Button]
        public void MakeBuild()
        {
            BuildWall(_result.buildableWalls[buildindex]);
        }
        [Button(0, 1, 1)]
        public void MakeRanodmBuild()
        {
            BuildWall(_result.buildableWalls[Random.Range(0, _result.buildableWalls.Count)]);
        }
        public void BuildWall(MapBuilder.PossibleBuildResult.BuildableWall buildableWall)
        {
            var wall = Wall.MakeNewWallAndAlignToLink(buildableWall.@from, buildableWall.to);
            walls.Add(wall);
            wall.transform.SetParent(wallsParent);
        }
    }
}