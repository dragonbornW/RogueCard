
using UnityEngine;
using UnityEngine.EventSystems;
using Worlds;
using Worlds.Battles;

namespace Cards {

    public class DeckTopMost : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

        private PointerEventData _pointer;

        private HandCardView _card_view;
        private HandCardSlotView _slot;
        private TargetArrow _arrow;
        private ActorView _target;

        public void init(HandCardSlotView slot, Vector2 position) {
            if (slot.view.card.desc.target == CardTarget.Monster) {

                var prefab = Resources.Load<TargetArrow>("target_arrow");
                if (prefab != null) {
                    //var root = WorldSceneRoot.instance;
                    _arrow = Instantiate(prefab, transform, false);
                    _arrow.cursor_point = position;
                    _arrow.card_point = slot.get_card_head_position();
                    _arrow.color = Color.red;
                }

            } else {
                var root = WorldSceneRoot.instance;
                _card_view = slot.take_card_view();
                _card_view.transform.SetParent(root.ui_main, false);
                _card_view.transform.position = position;
            }
            _slot = slot;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
            _pointer = eventData;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
            _pointer = null;
        }

        protected void Update() {
            if (_pointer != null) {
                if (_card_view != null) {
                    // 卡牌 跟随 光标位置 
                    _card_view.transform.position = _pointer.position;
                } else if (_arrow != null) {
   
                    _arrow.card_point = _slot.get_card_head_position();

                    var camera = WorldSceneRoot.instance.mainCamera;

                    var world_pos = (Vector2)camera.ScreenToWorldPoint(_pointer.position);

                    var collider = Physics2D.OverlapPoint(world_pos);

                    ActorView target = null;

                    if (collider != null) {
                        var actor_bounds = collider.GetComponent<ActorBounds>();
                        if (actor_bounds != null) {
                            target = actor_bounds.view;
                        }
                    }

                    if (target != _target) {
                        _target = target;
                        if (_target != null) {
                            _arrow.color = Color.green;
                        } else {
                            _arrow.color = Color.red;
                        }
                    }

                    if (_target != null) {
                        _arrow.cursor_point = camera.WorldToScreenPoint(_target.transform.position);
                    } else {
                        _arrow.cursor_point = _pointer.position;
                    }
                }
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                if (_card_view != null) {
                    _slot.back_view(_card_view, eventData.position);
                }
                Destroy(gameObject);
                return;
            } 

            if (eventData.button == PointerEventData.InputButton.Left) {
                if (_card_view != null) {
                    _slot.use(_card_view, null);
                } else if (_target != null) {
                    _slot.use(null, _target.actor);
                }
                Destroy(gameObject);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            
        }
    }

}