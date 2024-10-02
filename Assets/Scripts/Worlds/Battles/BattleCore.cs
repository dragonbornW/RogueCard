

using Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Worlds.Battles {

    public class BattleCore {

        public Actor player = new Actor();
        public List<Monster> monsters = new List<Monster>();


        public int energy;
        public int energy_max;

        public bool player_wanna_end_round;

        public readonly List<Card> draw_pile = new List<Card>();
        public readonly List<Card> discard_pile = new List<Card>();
        public readonly List<Card> hand_pile = new List<Card>();

        private ActorSeat _player_seat;
        private ActorSeat _monster_seat;

        private State _state;

        private IBattleAction _current_action;
        private Stack<IEnumerator> _incomings = new Stack<IEnumerator>();

        public enum State {
            Initial = 0,
            BattleBegin,
            PlayerRoundBegin,
            PlayerRound,
            PlayerRoundEnd,
            MonsterRoundBegin,
            MonsterRound,
            MonsterRoundEnd,
            BattleEnd,
        }

        public State state => _state;

        public void init(WorldBattleState state) {

            var root = WorldSceneRoot.instance;

            var wp = WorldState.current.player;

            // 初始化玩家角色数据
            player.hp_max = wp.hp_max;
            player.hp = wp.hp;
            // 创建玩家角色外观
            var actor_view_prefab = Resources.Load<ActorView>("player");
            player.view = Object.Instantiate(actor_view_prefab, root.transform, false);
            player.view.init(player);

            var player_seat_name = string.IsNullOrEmpty(state.node.desc.player_seat) ? "player" : state.node.desc.player_seat;
            if (root.seats.TryGetValue(player_seat_name, out _player_seat) ) {
                _player_seat.add(player.view);
            }

            var monster_seat_name = string.IsNullOrEmpty(state.node.desc.monster_seat) ? "monster" : state.node.desc.monster_seat;
            root.seats.TryGetValue(monster_seat_name, out _monster_seat);

            foreach (var monster_id in state.node.desc.monsters) {
                if (Monsters.MonsterDesc.all_monsters.TryGetValue(monster_id, out var desc)) {
                    var monster = new Monster(desc);
                    monsters.Add(monster);
                    actor_view_prefab = Resources.Load<ActorView>(desc.view_prefab);
                    if (actor_view_prefab != null) {
                        monster.view = Object.Instantiate(actor_view_prefab, root.transform, false);
                        monster.view.init(monster);
                        _monster_seat?.add(monster.view);
                    }
                } else {
                    Debug.LogError($"monster  `{monster_id}` not exist");
                }
            }

            energy_max = wp.energy_max;

            foreach (var card_desc in wp.cards) {
                draw_pile.Add(new Card(card_desc));
            }

            shuffle(draw_pile);

            state.ui_root.update_energy(energy);

            _state = State.Initial;
        }

        public void update(WorldBattleState battle_state) {
            if (_current_action != null) {
                if (!_current_action.done) {
                    // 继续等待 当前行为完成
                    return;
                }
                _current_action = null;
            }

            // 没有当前行为
            while (_incomings.Count > 0) {
            l_enumrate_action:
                // 获得行为枚举器
                var incoming = _incomings.Peek();
                // 尝试获得下一个行为或行为枚举器
                while (incoming.MoveNext()) {
                    // 存在下一个
                    var obj = incoming.Current;
                    
                    if (obj is IBattleAction action) {
                        // 如果是 action
                        // 开始 action
                        action.start(battle_state);
                        // 如果 action 没有 完成
                        if (!action.done) {
                            // 保存下来
                            _current_action = action;
                            // 下一次 update 检测
                            return;
                        }
 
                        
                    } else if (obj is IEnumerator iter) {
                        // 如果是 行为枚举器，则保存下来（压栈）
                        _incomings.Push(iter);

                        goto l_enumrate_action;
                    }
                }
                _incomings.Pop();
            }

            // 没有任何后续行为了
            next_state(battle_state);
        }

        private void next_state(WorldBattleState battle_state) {
            switch (_state) {
                case State.Initial:
                    _state = State.BattleBegin;
                    // 执行在BattleBegin的所有事件
                    break;
                case State.BattleBegin:
                    _state = State.PlayerRoundBegin;
                    _current_action = new BattleActionCoroutine(_begin_player_round(battle_state));
                    _current_action.start(battle_state);

                    break;
                case State.PlayerRoundBegin:
                    _state = State.PlayerRound;
                    battle_state.ui_root.handCardDeck.mark_dirty();
                    break;
                case State.PlayerRound: {
                    check_monsters();
                    if (check_result()) {
                        if (player_wanna_end_round) {
                            player_wanna_end_round = false;
                            _state = State.PlayerRoundEnd;
                            _current_action = new BattleActionCoroutine(_end_player_round(battle_state));
                            _current_action.start(battle_state);
                        }
                    }
                    break;
                }
                case State.PlayerRoundEnd: {
                    
                    foreach (var monster in monsters) {
                        monster.on_round_begin(this);
                    }
                    _state = State.MonsterRoundBegin;
                    break;
                }
                case State.MonsterRoundBegin:
                    _state = State.MonsterRound;
                    _incomings.Push(_on_monster_round());
                    break;
                case State.MonsterRound:
                    _state = State.MonsterRoundEnd;
                    foreach (var monster in monsters) {
                        monster.on_round_end(this);
                    }
                    break;
                case State.MonsterRoundEnd:
                    _state = State.PlayerRoundBegin;
                    _current_action = new BattleActionCoroutine(_begin_player_round(battle_state));
                    _current_action.start(battle_state);
                    break;
                case State.BattleEnd:
                    break;
            }
        }

        private IEnumerator _begin_player_round(WorldBattleState battle_state) {



            var wp = WorldState.current.player;
            for (int i = 0; i < wp.draw_card_count; ++i) {

                if (draw_pile.Count == 0) {
                    if (discard_pile.Count == 0) {
                        break;
                    }
                    foreach (var card in discard_pile) {
                        draw_pile.Add(card);
                    }
                    discard_pile.Clear();
                    shuffle(draw_pile);
                }
                {
                    var last = draw_pile.Count - 1;
                    var card = draw_pile[last];
                    draw_pile.RemoveAt(last);

                    hand_pile.Add(card);

                    battle_state.ui_root.handCardDeck.add(card);

                    yield return new WaitForSeconds(0.125f);
                }
            }

            energy = energy_max;
            battle_state.ui_root.update_energy(energy);

            battle_state.ui_root.show_end_round();

            player.on_round_begin(this);

            foreach (var monster in monsters) {
                monster.generate_intent(this);
            }
        }

        private IEnumerator _end_player_round(WorldBattleState battle_state) {

            player.on_round_end(this);

            battle_state.ui_root.handCardDeck.clear();
            foreach (var card in hand_pile) {
                discard_pile.Add(card);
            }
            hand_pile.Clear();
            yield break;
        }

        private IEnumerator _on_monster_round() {
            var monsters = this.monsters.ToArray();
            foreach (var monster in monsters) { 
                if (monster.alive) {
                    yield return monster.preform_intent(this);
                }
                check_monsters();
                if (!check_result()) {
                    break;
                }
            }
        }

        public static void shuffle(List<Card> cards) {
            for (int i = 0, c = cards.Count / 2; i < c; ++i) {
                var idx1 = Random.Range(0, cards.Count);
                var idx2 = Random.Range(0, cards.Count);
                var t = cards[idx1];
                cards[idx1] = cards[idx2];
                cards[idx2] = t;
            }
        }

        public void use_card(WorldBattleState battle_state, Card card, Actor target) {
            _incomings.Push(_card_using(battle_state, card, target));
        }

        private IEnumerator _card_using(WorldBattleState battle_state, Card card, Actor target) {
            hand_pile.Remove(card);
            battle_state.ui_root.handCardDeck.remove(card);

            if (card.desc.cost_type == CardCostType.AllRemains) {
                card.action_cost = energy;
                energy = 0;
            } else {
                card.action_cost = card.desc.fixed_cost;
                energy -= card.action_cost;
            }

            battle_state.ui_root.update_energy(energy);

            yield return card.desc.real_action.perform(battle_state, card, target);

            discard_pile.Add(card);

            //yield break;
        }

        private void check_monsters() {
            for (var i = monsters.Count - 1; i >= 0; --i) {
                var monster = monsters[i];
                if (!monster.alive) {
                    monsters.RemoveAt(i);
                    _monster_seat.remove(monster.view);
                    monster.destroy();
                }
            }
        }

        private bool check_result() {
            if (!player.alive || monsters.Count == 0) {
                player.destroy();

                foreach (var monster in monsters) {
                    monster.destroy();
                }
                monsters.Clear();

                _player_seat.clear();
                _monster_seat.clear();

                _current_action = null;
                _incomings.Clear();

                _state = State.BattleEnd;

                if (player.alive) {

                    var world = WorldState.current;
                    world.map.make_current_done();
                    world.next_sub_state = new WorldMapState();

                } else {

                    // TODO:

                }

                return false;
            }
            return true;
        }
    }

}