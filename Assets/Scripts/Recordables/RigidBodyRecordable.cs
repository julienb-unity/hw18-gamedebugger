using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
	[Serializable]
	public class RigidBodyRecordable : Recordable<Rigidbody>
	{
		[SerializeField]
		private Vector3 m_Speed;
		public Vector3 speed
		{
			get { return m_Speed; }
		}

		public override bool OnRecord(Recordable previous, Object source)
		{
			var rigidBody = source as Rigidbody;
			var prev = previous as RigidBodyRecordable;

			if (prev != null && prev.m_Speed == rigidBody.velocity)
				return false;
			
			m_Speed = rigidBody.velocity;
			return true;
		}

		public override void OnReplay(Object source)
		{
			var rigidBody = source as Rigidbody;
			rigidBody.velocity = m_Speed;
		}

		public bool ApproximatelyEquals(RigidBodyRecordable other)
		{
			return Mathf.Abs(m_Speed.sqrMagnitude - other.m_Speed.sqrMagnitude) <= 0.001f;
		}
	}
}
