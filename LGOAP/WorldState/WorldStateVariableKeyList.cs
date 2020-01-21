/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateVariableKeyList.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: List of keys for a WorldStateVariable.
 * *               Created this way to accomodate multiple
 * *               objects within one file.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using Hiralal.AdvancedPatterns.SOList;

using UnityEngine;

namespace Hiralal.LGOAP
{
    [CreateAssetMenu(fileName = "New_LGOAP_WSV_Keys_Database",menuName = "Hiralal/LGOAP/WorldState/Keys Database")]
    public class WorldStateVariableKeyList : ScriptableObjectList<WorldStateVariableKey> { }
}