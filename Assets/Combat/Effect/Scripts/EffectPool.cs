using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    public class EffectPool : Pixelplacement.Singleton<EffectPool>
    {
        [SerializeField] List<BaseEffectSO> effectDatas = new List<BaseEffectSO>();

        Dictionary<string, BaseEffectSO> effectDict = new Dictionary<string, BaseEffectSO>();

        protected override void OnRegistration()
        {
            foreach (BaseEffectSO effect in effectDatas)
            {
                effectDict.TryAdd(effect.effectId, effect);
            }
        }

        public BaseEffectSO GetEffectSO(string id)
        {
            if (effectDict.TryGetValue(id, out BaseEffectSO effect))
            {
                return effect;
            }
            return null;
        }
    }
}
