using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

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
            if (screenshot.screenshot != null)
            {
                var width = screenshot.screenshot.width * 45 / screenshot.screenshot.height;
                return new Rect(pixel, 0, width, 45);
            }
            return Rect.zero;
        }

        public override void OnClick(VisualElement panel, float time)
        {
            var screenshot = m_Screenshots.LastOrDefault(i => time > i.time);
            var box = new Image();
            panel.Add(box);
            box.scaleMode = ScaleMode.ScaleToFit;
            box.image = screenshot.screenshot;
            box.style.width = 250;
            box.style.height = panel.contentRect.width * screenshot.screenshot.height / screenshot.screenshot.width;
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
