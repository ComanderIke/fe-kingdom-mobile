using Assets.GameActors.Units;
using Assets.GameActors.Units.CharStateEffects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GUI
{
    public class BuffUi : MonoBehaviour
    {
        private Dictionary<string, GameObject> buffs;

        private void OnEnable()
        {
            buffs = new Dictionary<string, GameObject>();
        }

        public void Initialize(Unit unit)
        {
            unit.OnBuffAdded += AddBuff;
            unit.OnDebuffAdded += AddBuff;
            unit.OnBuffRemoved += RemoveBuff;
            unit.OnDebuffRemoved += RemoveBuff;
        }
        void AddBuff(CharacterState state)
        {
            if (!buffs.ContainsKey(state.name))
            {
                GameObject buff = Instantiate(state.Visual, transform);
                buffs.Add(state.name, buff);
            }
        }
        void RemoveBuff(CharacterState state)
        {
            if (buffs.ContainsKey(state.name))
            {
                GameObject.Destroy(buffs[state.name]);
                buffs.Remove(state.name);
            }
        }

    }
}