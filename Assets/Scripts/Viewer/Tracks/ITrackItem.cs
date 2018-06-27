namespace GameDebugger
{
    interface ITrackItem
    {
        void Draw(Track track, ITimeConverter converter);
        void Refresh(RecordableInfo recordableInfo, int frame);
    }
}
