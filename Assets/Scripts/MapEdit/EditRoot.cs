using System.Collections.Generic;
using UnityEngine;
namespace MapEdit {
    public class EditRoot : MonoBehaviour {
        
        public MapAsset asset;


        [ContextMenu("Save Asset")]
        private void save_asset() {
            if (asset == null) {
                return;
            }
            var nodes = GetComponentsInChildren<EditNode>();

            asset.entries = new List<int>();

            var index = 0;
            foreach (var node in nodes) {
                node.index = index;
                if (node.isEntry) {
                    asset.entries.Add(index);
                }
                ++index;
            }
            asset.nodes = new List<MapAsset.Node>();

            asset.height = 0;

            foreach (var node in nodes) {
                var obj = node.create_node();
                obj.position = node.transform.position;
                if (obj.position.y > asset.height) {
                    asset.height = obj.position.y;
                }
                var nexts = new List<int>();
                foreach (var next in node.nexts) {
                    if (next != null) {
                        nexts.Add(next.index);
                    }
                }
                obj.next_indices = nexts.ToArray();
                asset.nodes.Add(obj);
            }

            UnityEditor.EditorUtility.SetDirty(asset);
        }

    }

}



