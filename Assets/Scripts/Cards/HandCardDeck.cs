using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Worlds;

namespace Cards {

    public class HandCardDeck : UIBehaviour {

        public HandCardView cardPrefab;
        public Transform content;
        public Transform smallCenter;

        public float smallScale = 0.75f;
        public float smallOverlap = 0.2f;
        public float curve = 0.00025f;

        private Vector2 _card_size;
        private List<HandCardSlotView> _slots = new List<HandCardSlotView>();
        private bool _dirty;
        private Rect _content_rect;

        private HandCardSlotView _hovered;

        private WorldBattleState _battle_state;

        public Vector2 card_size => _card_size;

        public WorldBattleState battle_state => _battle_state;

        protected override void Awake() {
            _card_size = (cardPrefab.transform as RectTransform).rect.size;

            _content_rect = (content.transform as RectTransform).rect;
        }

        public void init(WorldBattleState battle_state) {
            _battle_state = battle_state;
        }

        public void mark_dirty() {
            _dirty = true;
        }

        public void slot_hover(HandCardSlotView slot) {
            if (_hovered != slot) {
                _hovered = slot;
                _dirty = true;
            }
        }

        public void slot_unhover(HandCardSlotView slot) {
            if (_hovered == slot) {
                _hovered = null;
                _dirty = true;
            }
        }

        public void slot_down(HandCardSlotView slot, Vector2 position) {
            if (_battle_state.core.state != Worlds.Battles.BattleCore.State.PlayerRound) {
                return;
            }

            if (!slot.view.card.validate(_battle_state.core)) {
                return;
            }

            var root = WorldSceneRoot.instance;
            var topmost = new GameObject("[Topmost]").AddComponent<RectTransform>();
            topmost.SetParent(root.ui_root.transform, false);
            topmost.anchorMin = Vector2.zero;
            topmost.anchorMax = Vector2.one;
            topmost.anchoredPosition = Vector2.zero;
            topmost.sizeDelta = Vector2.zero;

            var img = topmost.gameObject.AddComponent<Image>();
            img.color = new Color(1, 1, 1, 0);

            var obj = topmost.gameObject.AddComponent<DeckTopMost>();
            obj.init(slot, position);
            
        }

        public void add(Card card) {
            // ����Slot���󣬲�����HandCardSlotView���
            var slot = new GameObject("[Slot]").AddComponent<HandCardSlotView>();
            // ���� Image ����Ӧ�¼���
            var img = slot.gameObject.AddComponent<Image>();
            // ���� img �� ��ɫ��͸����ɫ
            img.color = new Color(1, 1, 1, 0);
            // �� slot ����  content ��
            slot.transform.SetParent(content, false);
            // ���� slot �� ��͸�
            var rt = slot.transform as RectTransform;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _card_size.x);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _card_size.y);

            // ���� ������� HandCardView
            var view = Instantiate(cardPrefab, slot.transform, false);
            view.init(card);
            
            // ���� slot �� view������ slot ���õ� center ��λ��
            slot.init(this, view, content.InverseTransformPoint(smallCenter.position), smallScale);
            _slots.Add(slot);
            _dirty = true;
        }

        public void remove(Card card) {
            for (int i = 0; i < _slots.Count; i++) {
                if (_slots[i].view.card == card) {
                    Destroy(_slots[i].gameObject);
                    _slots.RemoveAt(i);
                    _dirty = true;
                    break;
                }
            }
        }

        public void clear() {
            foreach (var slot in _slots) {
                Destroy(slot.gameObject);
            }
            _slots.Clear();
            _dirty = true;
        }

        protected void Update() {
            
        }

        protected void LateUpdate() {
            if (_dirty) {
                _dirty = false;
                update_cards();
            }
        }

        private void update_cards() {
            if (_slots.Count == 0) {
                return;
            }
            // ���ĵ�
            var center_pos = (Vector2)content.InverseTransformPoint(smallCenter.position);

            // ��С�Ŀ��ƴ�С
            var small_size = _card_size * smallScale;

            // �ص���Ŀ��ƿ��
            var overlapped_width = small_size.x * (1 - Mathf.Clamp01(smallOverlap));

            var width = _slots.Count * overlapped_width;
            if (width > _content_rect.width) {
                overlapped_width = _content_rect.width / _slots.Count;
                width = _content_rect.width;
            }

            var offset = -(width * 0.5f) + overlapped_width * 0.5f;

            int index = 0;
            int hovered_index = -1;
            foreach (var slot in _slots) {
                Vector2 t;
                var angle = -offset * curve;
                if (angle != 0) {
                    var cos = Mathf.Cos(angle);
                    var sin = Mathf.Sin(angle);
                    t = new Vector2(cos * offset, sin * offset);
                } else {
                    t = new Vector2(offset, 0);
                }
                slot.position = center_pos + t;

                slot.set_scale(smallScale);

                slot.set_rotation(angle * Mathf.Rad2Deg);

                slot.transform.SetSiblingIndex(index);

                if (_hovered == slot) {
                    hovered_index = index;
                }

                ++index;
                offset += overlapped_width;
            }


            if (_battle_state.core.state == Worlds.Battles.BattleCore.State.PlayerRound && _hovered != null) {
                var t = _hovered.transform;
                _hovered.set_scale(1, false);
                _hovered.set_rotation(0, false);
                _hovered.reset_position_y(_content_rect.yMin + _card_size.y * 0.5f);
                _hovered.transform.SetAsLastSibling();

                var w = (_card_size.x - small_size.x) * 0.5f + small_size.x - overlapped_width;
                var off = w;
                for (int i = hovered_index - 1; i >= 0; --i) {
                    var slot = _slots[i];
                    var pos = slot.position;
                    pos.x -= off;
                    slot.position = pos;
                    off *= 0.75f;
                }
                off = w;
                for (int i = hovered_index + 1; i < _slots.Count; ++i) {
                    var card = _slots[i];
                    var pos = card.position;
                    pos.x += off;
                    card.position = pos;
                    off *= 0.75f;
                }
            }
        }
    }

}