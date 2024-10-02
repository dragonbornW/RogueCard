
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards {

    public class TargetArrow : UIBehaviour {

        public Image[] nodes;


        private bool _dirty = false;

        private Vector2 _card_point;
        private Vector2 _cursor_point;
        private Color _color;

        public Vector2 card_point {
            get => _card_point;
            set {
                if (_card_point != value) {
                    _card_point = value;
                    _dirty = true;
                }
            }
        }

        public Vector2 cursor_point {
            get => _cursor_point;
            set {
                if (_cursor_point != value) {
                    _cursor_point = value;
                    _dirty = true;
                }
            }
        }

        public Color color {
            get => _color;
            set {
                if (_color != value) {
                    _color = value;
                    foreach (var node in nodes) {
                        node.color = color;
                    }
                }
            }
        }

        protected void Update() {
            if (_dirty) {
                _dirty = false;
                var last = nodes.Length - 1;
                if (last > 0) {
                    for (int i = 0; i <= last; ++i) {
                        var f = (float)i / last;
                        nodes[i].transform.position = Vector2.LerpUnclamped(_card_point, _cursor_point, f);
                    }
                }
            }
        }
    }

}