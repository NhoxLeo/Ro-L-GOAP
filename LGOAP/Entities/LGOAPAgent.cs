/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: LGOAPAgent.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: AI Agent.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using Hiralal.AdvancedPatterns.ScriptableObjectVariables;
using UnityEngine;

namespace Hiralal.LGOAP
{
    public class LGOAPAgent : LGOAPEntity
    {
        public WorldStateVariableKey AgentLocationToken { get; private set; }

        [SerializeField] private Goal[] goals;

        private void Awake()
        {
            AgentLocationToken = ScriptableObject.CreateInstance<WorldStateVariableKey>();
            AgentLocationToken.name = "Runtime Token (" + AgentLocationToken.InstanceID + ")";
        }
    }
}