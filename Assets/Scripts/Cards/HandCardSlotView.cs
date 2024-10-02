
using UnityEngine;
using UnityEngine.EventSystems;
using Worlds.Battles;

namespace Cards {
    public class HandCardSlotView : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

        private HandCardView _view;
        private HandCardDeck _deck;
        
        // 期望的scale
        private float _scale = 1;
        // 当前的scale
        private float _scale_current = 1;
        // scale变化的速度
        private float _scale_speed = 0;

        private Vector2 _position;
        private Vector2 _position_current;
        private Vector2 _position_speed;
        private float _rotation;
        private float _rotation_speed;
        private float _rotation_current;
        private Vector2 _position_backing;
        private float _backing_timer;
        private bool _backing;

        public HandCardView view => _view;

        public Vector2 position {
            get => _position;
            set {
                _position = value;
                //transform.localPosition = position;
            }
        }

        public HandCardView take_card_view() {
            var t = _view;
            _view = null;
            return t;
        }

        public void init(HandCardDeck deck, HandCardView view, Vector2 position, float scale) {
            _deck = deck;
            _view = view;
            _position = _position_current = position;
            _scale = _scale_current = scale;
            _rotation = _rotation_current = _rotation;

            var t = transform;
            t.localPosition = _position;
            t.localScale = new Vector3(_scale, _scale, 1);
            t.localEulerAngles = new Vector3(0, 0, _rotation);
        }

        public Vector2 get_card_head_position() {
            var off = new Vector2(0, _deck.card_size.y * 0.5f);
            return transform.TransformPoint(off);
        }

        public void reset_position_y(float y) {
            _position.y = y;
            _position_current.y = y;
            _position_speed.y = 0;

            transform.localPosition = _position;
        }

        public void set_scale(float scale, bool fading = true) {
            _scale = scale;
            if (!fading) {
                _scale_current = _scale;
                transform.localScale = new Vector3(scale, scale, 1);
                _scale_speed = 0;
            }
        }

        public void set_rotation(float rotation, bool fading = true) {
            _rotation = rotation;
            if (!fading) {
                _rotation_current = _rotation;
                transform.localEulerAngles = new Vector3(0, 0, rotation);
                _rotation_speed = 0;
            }
        }

        public void back_view(HandCardView view, Vector2 position) {
            _view = view;
            _backing = true;
            _backing_timer = 0;
            _position_backing = position;
        }

        public void use(HandCardView view, Actor target) {
            if (_view == null) {
                _view = view;
                _view.transform.SetParent(transform, false);
                _view.transform.localPosition = Vector2.zero;
                _view.transform.localScale = Vector3.one;
                _view.transform.localEulerAngles = Vector3.zero;
            }
            _deck.battle_state.core.use_card(_deck.battle_state, _view.card, target);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
            _deck.slot_hover(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
            _deck.slot_unhover(this);
        }

        protected void Update() {
            const float TIME = 0.125f;

            if (_position_current != _position) {
                _position_current = Vector2.SmoothDamp(_position_current, _position, ref _position_speed, TIME);
                transform.localPosition = _position_current;
            }

            if (_scale_current != _scale) {
                _scale_current = Mathf.SmoothDamp(_scale_current, _scale, ref _scale_speed, TIME);
                if (Mathf.Abs(_scale_current - _scale) <= 0.001f) {
                    _scale_current = _scale;
                }
                transform.localScale = new Vector3(_scale_current, _scale_current, 1);
            }

            if (_rotation_current != _rotation) {
                _rotation_current = Mathf.SmoothDamp(_rotation_current, _rotation, ref _rotation_speed, TIME);
                transform.localEulerAngles = new Vector3(0, 0, _rotation_current);
            }

            if (_backing) {
                _backing_timer += Time.deltaTime;
                if (_backing_timer >= TIME) {
                    _backing = false;
                    _view.transform.SetParent(transform, false);
                    _view.transform.localPosition = Vector2.zero;
                    _view.transform.localScale = Vector3.one;
                    _view.transform.localEulerAngles = Vector3.zero;

                } else {
                    var f = _backing_timer / TIME;
                    _view.transform.position = Vector2.LerpUnclamped(_position_backing, transform.position, f);

                    var s = Mathf.LerpUnclamped(1, _scale_current, f);
                    _view.transform.localScale = new Vector3(s, s, 1);

                    var r = Mathf.LerpUnclamped(0, _rotation_current, f);
                    _view.transform.localEulerAngles = new Vector3(0, 0, r);
                }
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (_backing) {
                    _backing = false;
                    _view.transform.SetParent(transform, false);
                    _view.transform.localPosition = Vector2.zero;
                    _view.transform.localScale = Vector3.one;
                    _view.transform.localEulerAngles = Vector3.zero;
                }
                _deck.slot_down(this, eventData.position);
            }
        }
    }
}