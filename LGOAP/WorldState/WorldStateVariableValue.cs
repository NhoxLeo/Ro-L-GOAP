/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateVariableValue.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Extendable enum to be used as a
 * *               value for world-state variables.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;

namespace Hiralal.LGOAP
{
    public class WorldStateVariableValue : ScriptableObject
    {
        [SerializeField] private byte index = default;
        public byte Index => index;

        public override string ToString() => name + " (" + index + ")";

        public static implicit operator byte(WorldStateVariableValue value) => value.index;
    }
}