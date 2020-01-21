/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: LGOAPEntity.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Abstraction for any GOAP entity
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;
using Hiralal.AdvancedPatterns.ScriptableObjectVariables;
using UnityEngine;

namespace Hiralal.LGOAP
{
    public abstract class LGOAPEntity : MonoBehaviour
    {
        private readonly Dictionary<int, byte> worldState = new Dictionary<int, byte>();
        
        public void SetState(WorldStateVariableKey key, byte value) => worldState[key.InstanceID] = value;

        public byte GetState(WorldStateVariableKey key) => 
            !worldState.ContainsKey(key.InstanceID) ? (byte) 0 : worldState[key.InstanceID];
    }
}