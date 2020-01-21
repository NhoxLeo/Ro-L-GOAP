/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: WorldStateVariableValueList.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: List of values for a WorldStateVariable.
 * *               Created this way to accomodate multiple
 * *               objects within one file.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using Hiralal.AdvancedPatterns.SOList;
using UnityEngine;

namespace Hiralal.LGOAP
{
    [CreateAssetMenu(fileName = "New_LGOAP_WSV_Values_Database",menuName = "Hiralal/LGOAP/WorldState/Values Database")]
    public class WorldStateVariableValueList : ScriptableObjectList<WorldStateVariableValue> { }
}