using UnityEngine;

namespace Recordables
{
	public class RigidBodyRecordable : Recordable<Rigidbody>
	{
		[SerializeField] private Vector3 speed;
		public override void OnRecord(Object source)
		{
			var rigidBody = source as Rigidbody;
			speed = rigidBody.velocity;
		}

		public override void OnReplay(Object source)
		{
			var rigidBody = source as Rigidbody;
			rigidBody.velocity = speed;
		}
	}
}
