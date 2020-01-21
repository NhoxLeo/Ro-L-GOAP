/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateVariable.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: A single world-state variable.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;

namespace Hiralal.LGOAP
{
    [System.Serializable]
    public struct WorldStateVariable
    {
        /// <summary>
        /// 0: Primary AI Agent
        /// 1: Global
        /// 2: Primary Smart Object
        /// 3: Helper Entity 1
        /// 4: Helper Entity 2
        /// 5: Helper Entity 3
        /// </summary>
        public byte entityIndex;
        
        public WorldStateVariableKey key;
        public WorldStateVariableValue value;
    }
}