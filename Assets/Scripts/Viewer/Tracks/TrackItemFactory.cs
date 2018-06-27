using Recordables;

namespace GameDebugger
{
    static class TrackItemFactory
    {
        public static ITrackItem Create(RecordableInfo recordableInfo, int frame)
        {
            if (recordableInfo.recordable is TransformRecordable)
                return new TransformRecordableItem(recordableInfo.instanceID, frame);
            else return null;
        }
    }
}
