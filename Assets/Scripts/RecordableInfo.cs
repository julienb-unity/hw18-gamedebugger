using Recordables;

namespace GameDebugger
{
	public class RecordableInfo
	{
		public int instanceID;
		public Recordable recordable;

		public RecordableInfo(int instanceID, Recordable recordable)
		{
			this.instanceID = instanceID;
			this.recordable = recordable;
		}
	}
}