/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: LGOAPObject.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: An object that interacts with GOAP Agents.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using UnityEngine;

namespace Hiralal.LGOAP
{
    [RequireComponent(typeof(SphereCollider))]
    public class LGOAPObject : LGOAPEntity
    {
        private void OnTriggerEnter(Collider other)
        {
            var agent = other.GetComponent<LGOAPAgent>();
            if (agent == null) return;
            
            SetState(agent.AgentLocationToken, 1);
        }

        private void OnTriggerExit(Collider other)
        {
            var agent = other.GetComponent<LGOAPAgent>();
            if (agent == null) return;
            
            SetState(agent.AgentLocationToken, 0);
        }
    }
}