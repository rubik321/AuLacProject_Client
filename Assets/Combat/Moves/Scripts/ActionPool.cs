using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class ActionPool : Pixelplacement.Singleton<ActionPool>
    {
        public List<BaseActionSO> actionDatas = new List<BaseActionSO>();

        Dictionary<string, BaseActionSO> actionDict = new Dictionary<string, BaseActionSO>();

        protected override void OnRegistration()
        {
            foreach (BaseActionSO action in actionDatas)
            {
                actionDict.TryAdd(action.actionId, action);
            }
        }

        public ActionResponse GetActionRespone(ActionRequest data)
        {
            foreach (BaseActionSO action in actionDatas)
            {
                if (action.actionId == data.actionId)
                {
                    return new ActionResponse()
                    {
                        actionId = action.actionId,
                        damageFormula = action.GetDamage,
                        effects = action.effects,
                        attacker = data.attacker,
                        mainTarget = data.target,
                        cost = action.cost,
                        targets = action.GetTargets(data.target),
                        isLongRange = action.isLongRange,
                        attackAnimationInfo = action.attackAnimationInfo
                    };
                }
            }
            return null;
        }

        public BaseActionSO GetActionSO(string id)
        {
            if (actionDict.TryGetValue(id, out BaseActionSO action))
            {
                return action;
            }
            return null;
        }
    }
}
