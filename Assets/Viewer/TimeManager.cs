using System;
using UnityEngine;

namespace GameDebugger 
{
    class TimeManager
    {
        int m_Time;
        
        public int time 
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