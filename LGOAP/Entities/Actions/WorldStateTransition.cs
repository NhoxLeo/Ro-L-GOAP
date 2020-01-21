/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateTransition.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: A single transition between two world-states.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;

namespace Hiralal.LGOAP
{
    [CreateAssetMenu(fileName = "New WorldStateTransition",menuName = "Hiralal/LGOAP/WorldStateTransition")]
    public class WorldStateTransition : ScriptableObject
    {
        public WorldStateVariable[] conditions = null;
        public WorldStateVariable[] effects = null;
    }
}