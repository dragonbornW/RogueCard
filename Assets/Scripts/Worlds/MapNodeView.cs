using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Worlds {

    public class MapNodeView : UIBehaviour {

        public GameObject selected;
        public Button body;
        public MapNode node;
        
        public MapView map_view { get; protected set;}
        
        public void on_node_state_changed( MapNode.State state ) {

            if( state == MapNode.State.Selected ) {
                if( selected != null ) {
                    selected.SetActive( true );
                }
            }else if( state == MapNode.State.CanSelect ) {
                if( body != null ) {
                    body.interactable = true;
                }
            } else {
                if( body != null ) {
                    body.interactable = false;
                }
            }
        }

        public void on_click() {
            
            map_view.on_node_clicked( this );
        }

        public virtual void init( MapView map_view, MapNode node ) {

            this.node = node;
            this.map_view = map_view;
            transform.localPosition = node.position + new Vector2( 0, 48 );
            on_node_state_changed( node.state );
        }
    }
}