using System;
using System.Linq;
using UnityEngine;

namespace Base.MapItemTypes
{
    [CreateAssetMenu(fileName = "Blocks Container", menuName = "Guide/Blocks Container", order = 0)]
    public class BlocksContainer : ScriptableObject
    {
        public BlockView[] blockViewPrefabs;
        public BlockPoint blockPointPrefabs;

        public (BlockView view, float rotation) GetTheAppropriateBlockView(Vector3[] waypoints)
        {
            foreach (var blockView in blockViewPrefabs)
            {
                var (canSpawn, rotation) = blockView.CanSpawnFor(waypoints);
                if (canSpawn)
                    return (blockView, rotation: rotation);
            }
            return (null, 0);
        }
        public class InstantiatingViewInfo
        {
            public BlockView view;
            public float rotation;
        }
        
    }
}