using UnityEngine;

namespace Base.MapItemTypes
{
    [CreateAssetMenu(fileName = "Walls Container", menuName = "Guide/Walls Container", order = 0)]
    public class WallsContainer : ScriptableObject
    {
        public Wall[] walls;
    }
}