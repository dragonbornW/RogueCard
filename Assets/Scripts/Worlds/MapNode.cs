using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace Worlds {

    public abstract class MapNode {

        //不能被选择
        //已被选择
        //可被选择
        public enum State {
            Disabled = 0,
            Selected = 1,
            CanSelect = 2,
        }
        public State state;
        public readonly List<MapNode> nexts = new List<MapNode>();

        public abstract MapNodeView view { get; set; }
        public abstract Vector2 position {get; }

        public abstract MapNodeView load_prefab();

        public void change_state( State state ) {

            if( this.state != state ) {
            this.state = state;
            view.on_node_state_changed( state );
            }
        }

        public abstract void do_enter();
    }
}