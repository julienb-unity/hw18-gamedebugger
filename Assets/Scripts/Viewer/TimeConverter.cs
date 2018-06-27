using System;

namespace GameDebugger 
{
    interface ITimeConverter
    {
        float TimeToPixel(float time);
        float PixelToTime(float pixel);
    }
    
    class TimeConverter : ITimeConverter
    {
        TimeAreaGUI m_TimeAreaGUI;
        
        public TimeConverter(TimeAreaGUI timeArea)
        {
            m_TimeAreaGUI = timeArea;
        }
        
        public float TimeToPixel(float time)
        {
            return m_TimeAreaGUI.TimeToPixel(time);
        }

        public float PixelToTime(float pixel)
        {
            return m_TimeAreaGUI.PixelToTime(pixel);
        }
    }
}