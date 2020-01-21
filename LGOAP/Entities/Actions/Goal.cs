/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: Goal.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Wrapper for a WS Transition. Used as a Goal
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Linq;

namespace Hiralal.LGOAP
{
    [System.Serializable]
    public class Goal
    {
        public Goal(WorldStateTransition transition = null, LGOAPEntity[] entities = null)
        {
            this.transition = transition;
            this.entities = entities;
        }
        
        public WorldStateTransition transition;
        public LGOAPEntity[] entities;
        
        public bool IsValid =>
            transition.conditions.All(condition => 
                condition.value.Index == entities[condition.entityIndex].GetState(condition.key));

        public virtual Goal CloneWithAgentAs(LGOAPEntity agent)
        {
            var newEntities = (LGOAPEntity[]) entities.Clone();
            newEntities[0] = agent;
            return new Goal(transition, newEntities);
        }
    }
}