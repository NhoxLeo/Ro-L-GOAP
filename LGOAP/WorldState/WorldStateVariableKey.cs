/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateVariableKey.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Extendable enum to be used as a
 * *               key for world-state variables.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;

namespace Hiralal.LGOAP
{
    public class WorldStateVariableKey : ScriptableObject
    {
        public int InstanceID { get; private set; }
        private void OnEnable() => InstanceID = GetInstanceID();

        public override string ToString() => name;
    }
}