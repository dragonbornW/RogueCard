using System.Collections.Generic;
using UnityEngine;

namespace Worlds {

    public abstract class MapNode {

        public abstract Vector2 position{get; }
        public readonly List<MapNode> nexts = new List<MapNode>();
        public abstract MapNodeView load_prefab();
    }
}