using Recordables;

namespace GameDebugger
{
    static class TrackItemFactory
    {
        public static ITrackItem Create(RecordableInfo recordableInfo, int frame)
        {
            if (recordableInfo.recordable is TransformRecordable)
                return new TransformRecordableItem(recordableInfo, frame);
            if (recordableInfo.recordable is AnimatorRecordable)
                return new AnimatorRecordableItem(recordableInfo, frame);
            if (recordableInfo.recordable is RigidBodyRecordable)
                return new RigidBodyRecordableItem(recordableInfo, frame);
            return null;
        }
    }
}
