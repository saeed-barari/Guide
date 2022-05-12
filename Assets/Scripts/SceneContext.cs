using System;
using Base;
using Base.MapItemTypes;
using UnityEngine;

namespace DefaultNamespace
{
    public class SceneContext : MonoBehaviour
    {
        private static SceneContext _instance;

        public static SceneContext Instance
        {
            get
            {
                if(!_instance)
                    _instance = FindObjectOfType<SceneContext>();
                return _instance;
            }
        }
        
        public MapItemsContainer mapItemsContainer;
        public BlocksContainer blocksContainer;
        public BlocksMap blocksMap;
    }
}