
using System.Collections.Generic;
using UnityEngine;
namespace MapEdit {

    [CreateAssetMenu]
    public class MapAsset : ScriptableObject {

        [System.Serializable]
        public abstract class Node{

            public Vector2 position;
            public int[] next_indices;
            public abstract Worlds.MapNode create_node();
        }

        [System.Serializable]
        public class BattleNode : Node {

            public override Worlds.MapNode create_node() {
                return new Worlds.MapBattleNode( this );
            } 
        }

        [SerializeReference]
        public List<Node> nodes = new List<Node>();
        public List<int> entries = new List<int>();
        public float height;

        public List<Worlds.MapNode> create_nodes() {

            var nodes = new List<Worlds.MapNode>( this.nodes.Count );
            
            foreach( var node in this.nodes ) {
                nodes.Add( node.create_node() );
            }

            for( int i = 0; i < nodes.Count; i++ ) {
                var node = nodes[i];
                var desc = this.nodes[i];
                foreach( var index in desc.next_indices ) {
                    node.nexts.Add( nodes[index] );
                }
            }

            return nodes;
        }
    }
}
