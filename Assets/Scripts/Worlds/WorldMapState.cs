
namespace Worlds {

    public class WorldMapState : SubState {

        public override void enter(SubState last ) {

            WorldSenceRoot.instance.map_view.gameObject.SetActive( true );
        }
        public override void exit() {

            WorldSenceRoot.instance.map_view.gameObject.SetActive( false );
        }
    }
}