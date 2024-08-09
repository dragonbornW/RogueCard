using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Worlds {

    public class MapView : UIBehaviour { 
        
        public ScrollRect scrollRect;
        private Map _map;
        
        public void init( Map map ) {

            _map = map;

            var rt = scrollRect.content;
            var height = map.height + 96;
            rt.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical,height );

            foreach( var node in map.nodes ) {

                var prefab = node.load_prefab();
                var node_view = Instantiate( prefab, rt, false );
                node.view = node_view;
                node_view.init( this, node );
            }
        }

        public void on_node_clicked( MapNodeView node_view ) {

            var world = WorldState.current;
            if( world.sub_state is WorldMapState state ) {
                state.select_node( node_view.node );
            }
        }
    }

}