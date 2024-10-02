
using System.Collections.Generic;
using UnityEngine;
using Worlds.Battles.Powers;

namespace Worlds.Battles {

    public abstract class ActorPower {

        public static readonly Dictionary<string, System.Func<ActorPower>> creators = new Dictionary<string, System.Func<ActorPower>> {
            { Vulnerable.NAME, () => new Vulnerable() },
            { Flex.NAME, () => new Flex() },
        };

        protected ActorPowerView _view;

        public abstract string prefab_path { get; }

        // power 被首次添加
        public virtual void enter(BattleCore core, Actor owner, int value) {
            var prefab = Resources.Load<ActorPowerView>(prefab_path);
            if (prefab != null) {
                _view = Object.Instantiate(prefab, owner.view.panel.powerContent, false);
            }
        }

        // 该 power 已存在，被叠加
        public virtual void overlap(BattleCore core, Actor owner, int value) {

        }

        public virtual void exit(BattleCore core, Actor owner) { 
            if (_view != null) {
                Object.Destroy(_view.gameObject);
                _view = null;
            }
        }

        public virtual void on_round_begin(BattleCore core, Actor owner) { 
        
        }

        public virtual void on_round_end(BattleCore core, Actor owner) {

        }
    }

}