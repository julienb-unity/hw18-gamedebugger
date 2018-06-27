using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
	[Serializable]
	public class RigidBodyRecordable : Recordable<Rigidbody>
	{
		[SerializeField] private Vector3 speed;
		public override bool OnRecord(Recordable previous, Object source)
		{
			var rigidBody = source as Rigidbody;
			var prev = previous as RigidBodyRecordable;

			if (prev != null && prev.speed == rigidBody.velocity)
				return false;
			
			speed = rigidBody.velocity;
			return true;
		}

		public override void OnReplay(Object source)
		{
			var rigidBody = source as Rigidbody;
			rigidBody.velocity = speed;
		}
	}
}
