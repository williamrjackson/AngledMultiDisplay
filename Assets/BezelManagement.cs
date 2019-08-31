using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrj
{
	public class BezelManagement : MonoBehaviour {

		[Range(0f,1f)]
		public float bezelSize = 0;

		public Transform rotationParent;
		public Transform videoManager;
		public Transform screen2;
		public float screen2LocalXMaxBezel;
		public float screen2LocalXMinBezel;
		public Transform screen4;
		public float screen4LocalXMaxBezel;
		public float screen4LocalXMinBezel;
		public Transform screen6;
		public float screen6LocalXMaxBezel;
		public float screen6LocalXMinBezel;
		public Transform screen8;
		public float screen8LocalXMaxBezel;
		public float screen8LocalXMinBezel;

		private float cachedSize = -1f;

		// Use this for initialization
		void Start () 
		{
			bezelSize = PlayerPrefs.GetFloat("BezelSize", 0);
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
				{
					bezelSize = 1f;
				}
				else
				{
					bezelSize = Mathf.Min(bezelSize + .05f, 1f);
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
				{
					bezelSize = 0f;
				}
				else
				{
					bezelSize = Mathf.Max(bezelSize - .05f, 0f);
				}
			}
			
			if (bezelSize != cachedSize)
			{
				rotationParent.localScale = rotationParent.localScale.With(x:Utils.Remap(bezelSize, 0f, 1f, 1f, 1.1f), y:Utils.Remap(bezelSize, 0f, 1f, 1f, 1.1f));
				videoManager.localScale = Vector3.one * Utils.Remap(bezelSize, 0f, 1f, 1f, 1.1f);

				screen2.localPosition = screen2.localPosition.With(x: Utils.Remap(bezelSize, 0f, 1f, screen2LocalXMinBezel, screen2LocalXMaxBezel));
				screen4.localPosition = screen4.localPosition.With(x: Utils.Remap(bezelSize, 0f, 1f, screen4LocalXMinBezel, screen4LocalXMaxBezel));
				screen6.localPosition = screen6.localPosition.With(x: Utils.Remap(bezelSize, 0f, 1f, screen6LocalXMinBezel, screen6LocalXMaxBezel));
				screen8.localPosition = screen8.localPosition.With(x: Utils.Remap(bezelSize, 0f, 1f, screen8LocalXMinBezel, screen8LocalXMaxBezel));

				cachedSize = bezelSize;
			
				PlayerPrefs.SetFloat("BezelSize", bezelSize);
			}
		}
	}
}
