using UnityEngine;

namespace Base.MapItemTypes
{
    [CreateAssetMenu(fileName = "Blocks Container", menuName = "Guide/Blocks Container", order = 0)]
    public class BlocksContainer : ScriptableObject
    {
        public BlockView[] blockViewPrefabs;
        public BlockPoint blockPointPrefabs;
    }
}