using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class ActionManager : MonoBehaviour {

        public List<Action> actionSlots = new List<Action>();

        public ItemAction consumableItem;

        StateManager states;

        public void Init(StateManager sm) {
            states = sm;

            UpdateActionsOneHanded();
        }

        public void UpdateActionsOneHanded() {
            EmptyAllSlots();
            Weapon w = states.invManager.curWeapon;

            for (int i = 0; i < w.actions.Count; i++) {
                Action a = GetAction(w.actions[i].input);
                a.targetAnimation = w.actions[i].targetAnimation;
            }
        }

        public void UpdateActionsTwoHanded() {
            EmptyAllSlots();
            Weapon w = states.invManager.curWeapon;

            for (int i = 0; i < w.two_handedActions.Count; i++) {
                Action a = GetAction(w.two_handedActions[i].input);
                a.targetAnimation = w.two_handedActions[i].targetAnimation;
            }
        }

        void EmptyAllSlots() {
            for (int i = 0; i < System.Enum.GetNames(typeof(ActionInput)).Length; i++) {
                Action a = GetAction((ActionInput)i);
                a.targetAnimation = null;
            }
        }

        ActionManager() {
            for (int i = 0; i < System.Enum.GetNames(typeof(ActionInput)).Length; i++) {
                Action a = new Action();
                a.input = (ActionInput)i;
                actionSlots.Add(a);
            }
        }

        public Action GetActionSlot(StateManager sm) {
            ActionInput act_in = GetActionInput(sm);
            return GetAction(act_in);
        }

        Action GetAction(ActionInput inp) {
            for(int i = 0; i < actionSlots.Count; i++) {
                if (actionSlots[i].input == inp)
                    return actionSlots[i];
            }
            return null;
        }

        public ActionInput GetActionInput(StateManager sm) {
            ActionInput r = ActionInput.rb;

            if (sm.rb)
                return ActionInput.rb;
            if (sm.rt)
                return ActionInput.rt;
            if (sm.lb)
                return ActionInput.lb;
            if (sm.lt)
                return ActionInput.lt;

            return r;
        }

    }

    public enum ActionInput {
        rb, lb, rt, lt//,x//, a
    }

    [System.Serializable]
    public class Action {
        public ActionInput input;
        public string targetAnimation;
    }

    [System.Serializable]
    public class ItemAction {
        public string targetAnimation;
        public string item_id;
    }

}
