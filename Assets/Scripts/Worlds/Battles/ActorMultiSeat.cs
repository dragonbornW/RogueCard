

using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Battles {

    public class ActorMultiSeat : ActorSeat {

        public float space = 2;

        private List<Slot> _slots = new List<Slot>();
        private bool _dirty = false;
        

        private class Slot {
            public ActorView view;
            public Vector2 position;
            public Vector2 velocity;
            public void update() {
                Vector2 pos = view.transform.position;
                if (pos != position) {
                    pos = Vector2.SmoothDamp(pos, position, ref velocity, 0.125f);
                    view.set_position(pos);
                }
            }
        }

        public override void add(ActorView view) {
            view.set_position(transform.position);
            _slots.Add(new Slot {
                view = view,
                position = transform.position,
            });
            _dirty = true;
        }

        public override void remove(ActorView view) {
            for (int i = 0, c = _slots.Count; i < c; ++i) {
                var slot = _slots[i];
                if (slot.view == view) {
                    _slots.RemoveAt(i);
                    _dirty = true;
                    break;
                }
            }
        }

        public override void clear() {
            _slots.Clear();
        }

        public void Update() {
            if (_dirty) {
                _dirty = false;
                update_slots();
            }
            foreach (var slot in _slots) {
                slot.update();
            }
        }

        private void update_slots() {
            var width = _slots.Count * space;
            var offset = -width * 0.5f + space * 0.5f;

            Vector2 center = transform.position;
              
            foreach (var slot in _slots) {
                slot.position = center + new Vector2(offset, 0);
                offset += space;
            }
        }
    }

}