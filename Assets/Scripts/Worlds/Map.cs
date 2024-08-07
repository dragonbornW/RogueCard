using System.Collections.Generic;

namespace Worlds {

    public class Map {

        public readonly List<MapNode> _nodes;
        public readonly List<MapNode> _entries;
        public  MapNode _current;
        public readonly float height; 

        public Map( MapEdit.MapAsset asset ) {

            _nodes = asset.create_nodes();

            _entries = new List<MapNode>( asset.entries.Count );
            foreach( var index in asset.entries ) {
                _entries.Add( _nodes[index] );
            }

            height = asset.height;
        }
    }
}