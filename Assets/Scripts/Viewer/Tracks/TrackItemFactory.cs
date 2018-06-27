using Recordables;

namespace GameDebugger
{
    static class TrackItemFactory
    {
        public static ITrackItem Create(RecordableInfo recordableInfo, int frame)
        {
            if (recordableInfo.recordable is TransformRecordable)
                return new TransformRecordableItem(recordableInfo, frame);
            else if (recordableInfo.recordable is AnimatorRecordable)
                return new AnimatorRecordableItem(recordableInfo, frame);
            else return null;
        }
    }
}
