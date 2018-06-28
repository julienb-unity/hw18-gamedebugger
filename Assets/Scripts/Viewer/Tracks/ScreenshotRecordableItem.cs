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
                GUI.DrawTexture(GetRectForScreenshot(converter, screenshot), screenshot.screenshot);
            }
        }

        static Rect GetRectForScreenshot(ITimeConverter converter, ScreenshotForTime screenshot)
        {
            var pixel = converter.TimeToPixel(screenshot.time);
            var width = screenshot.screenshot.width * 45 / screenshot.screenshot.height;
            return new Rect(pixel, 0, width, 45);
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
