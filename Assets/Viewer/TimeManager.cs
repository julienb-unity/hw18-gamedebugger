using System;
using UnityEngine;

namespace GameDebugger 
{
    class TimeManager
    {
        double m_Time;
        
        public double time 
        {
            get
            {
                return m_Time;
            }
            set
            {
                m_Time = value;
            }
        }
    }
}