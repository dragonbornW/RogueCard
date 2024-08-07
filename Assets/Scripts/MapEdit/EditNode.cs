using System.Collections.Generic;
using UnityEngine;

namespace MapEdit {

    public abstract class EditNode : MonoBehaviour {

        public List<EditNode> nexts;
        public bool isEntry;
        [System.NonSerialized]
        public int index;
        
        public abstract MapAsset.Node create_node();

        private void OnDrawGizmos() {
            
            var start = transform.position;
            
            foreach( var node in nexts ) {
                if( node == null ) {
                    continue;
                }

                Gizmos.DrawLine( start, node.transform.position );
            }
        }
    }
}