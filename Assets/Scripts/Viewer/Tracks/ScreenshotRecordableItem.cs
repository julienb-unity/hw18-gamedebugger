using System.Collections.Generic;
using Recordables;
using UnityEngine;

namespace GameDebugger
{
    class ScreenshotRecordableItem : TrackItem
    {
        struct ScreenshotForTime
        {
            public Texture2D screenshot;
            public float time;
        }
        
        List<ScreenshotForTime> m_Screenshots = new List<ScreenshotForTime>();
        
        public ScreenshotRecordableItem(RecordableInfo recordableInfo, int frame)
            : base(recordableInfo.instanceID)
        {
            m_Screenshots.Add(GetScreenshotForFrame(recordableInfo.recordable, frame));
        } 
        
        static ScreenshotForTime GetScreenshotForFrame(Recordable recordable, int frame)
        {
            var screenshotRec = recordable as ScreenShotRecordable;
            if (screenshotRec == null) return new ScreenshotForTime();

            var screenShot = screenshotRec.tex;
            return new ScreenshotForTime{ time = GameDebuggerDatabase.GetRecords(frame).time, screenshot = screenShot};
        }

        protected override void DrawItem(Track track, ITimeConverter converter)
        {
            foreach (var screenshot in m_Screenshots)
            {
                //////////ASPECT RATIO
                var pixel = converter.TimeToPixel(screenshot.time);
                var rect = new Rect(pixel, 0, 100, 45);
                GUI.DrawTexture(rect, screenshot.screenshot);
            }
        }

        public override void Refresh(RecordableInfo recordableInfo, int frame)
        {
            m_Screenshots.Add(GetScreenshotForFrame(recordableInfo.recordable, frame));
        }

        protected override string ItemName()
        {
            return "Screenshots";
        }
    }
}
