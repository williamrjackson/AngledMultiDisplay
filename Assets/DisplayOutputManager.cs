using System.Collections.Generic;
using UnityEngine;

namespace Wrj
{
	public class DisplayOutputManager : MonoBehaviour 
	{
        private void Awake()
        {
            // See what displays we would like to use.
            List<int> assignedDisplayIndexes = new List<int>();
            foreach (Camera camera in FindObjectsOfType<Camera>())
            {
                // Only try once for displays connected by multiple cameras
                if (!assignedDisplayIndexes.Contains(camera.targetDisplay))
                {
                    assignedDisplayIndexes.Add(camera.targetDisplay);
                }
            }

            // For each display we're hoping to use (as determined above), activate a display.
            foreach (int thisDisplayIndex in assignedDisplayIndexes)
            {
                // If there's a display available
                if (Display.displays.Length > thisDisplayIndex)
                {
                    // Activate it.
                    Display.displays[thisDisplayIndex].Activate();
                }
            }
        }
    }
}

