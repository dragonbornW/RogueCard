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

            foreach( var node in map._nodes ) {

                var prefab = node.load_prefab();
                var node_view = Instantiate( prefab, rt, false );
                node_view.transform.localPosition = node.position + new Vector2( 0, 48 );
            }
        }
    }

}