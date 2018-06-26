using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Animations;
using UnityEngine;

namespace Recordables
{
	public class AnimatorRecordable : Recordable<Animator>
	{
		[SerializeField] private List<string> LayerNames = new List<string>();
		
		private MethodInfo mi;
		public AnimatorRecordable()
		{
			mi = typeof(Animator).GetMethod("GetAnimatorStateName", BindingFlags.NonPublic | BindingFlags.Instance);
		}
		public override bool OnRecord(Recordable previous, Object source)
		{
			var animator = source as Animator;
			var prev = previous as AnimatorRecordable;


			for (int i = 0; i < animator.layerCount; i++)
			{
				var result = mi.Invoke(animator, new object[] {i, true});
				LayerNames.Add(result as string);				
			}
			
			if (prev != null && prev.LayerNames.SequenceEqual(LayerNames))
				return false;
			
			return true;
		}

		public override void OnReplay(Object source)
		{
		}
	}
}
