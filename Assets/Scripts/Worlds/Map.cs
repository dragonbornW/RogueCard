using System.Collections.Generic;

namespace Worlds {

    public class Map {

        public readonly List<MapNode> nodes;
        public readonly List<MapNode> entries;
        public  MapNode current;
        public readonly float height; 

        public Map( MapEdit.MapAsset asset ) {

            nodes = asset.create_nodes();

            entries = new List<MapNode>( asset.entries.Count );
            foreach( var index in asset.entries ) {
                entries.Add( nodes[index] );
            }

            height = asset.height;

            foreach( var entry in entries ) {
                entry.state = MapNode.State.CanSelect;
            }
        }

        public bool select_node( MapNode node) {

            if( node.state != MapNode.State.CanSelect) {
                return false;
            }  

            var old = current;
            current = node;
            current.change_state( MapNode.State.Selected );

            if( old != null ) {

                foreach( var next in old.nexts ) {
                    if( next != node ) {
                        next.change_state( MapNode.State.Disabled );
                    }
                }
            } else {
                foreach( var entry in entries ) {
                    if( entry != node ) {
                        entry.change_state( MapNode.State.Disabled );
                    }
                }
            }

            return true;
        }

        public void make_current_done() {

            if( current == null ) {
                return;
            }

            foreach( var next in current.nexts ) {
                next.change_state( MapNode.State.CanSelect );
            }
        }
    }
}