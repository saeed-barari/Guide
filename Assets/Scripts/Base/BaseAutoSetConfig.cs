using UnityEngine;

namespace Base
{
    [CreateAssetMenu(fileName = "Base Auto Set Config", menuName = "Guide/Base Auto Set Config")]
    public class BaseAutoSetConfig : ScriptableObject
    {
        public float reSpawnTime = 2;
        public float maxDistance = 50;
    }
}