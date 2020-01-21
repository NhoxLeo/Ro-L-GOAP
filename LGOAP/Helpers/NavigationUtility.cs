/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: NavigationUtility.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Provides path length calculation utility for Navigation.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using UnityEngine;
using UnityEngine.AI;

namespace Hiralal.LGOAP
{
    public static class NavigationUtility
    {
        public static (float, NavMeshPath) GetPathWithLength(Vector3 start, Vector3 end)
        {
            var path = new NavMeshPath();
            if (!NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path)) return (-1, null);

            var cornerLength = path.corners.Length;
            if (cornerLength == 1)
                return (Vector3.Distance(start, end), path);

            var length = 0.0f;
            for (int i = 1; i < cornerLength; i++) 
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);

            return (length, path);
        }
    }
}