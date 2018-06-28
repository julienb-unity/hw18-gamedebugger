using System;
using System.Collections.Generic;
using Recordables;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace GameDebugger
{
    class InputRecordableItem : TrackItem
    {
        struct InputsForTime
        {
            public float time;
            public List<string> inputs;

            public InputsForTime(float time, List<string> inputs)
            {
                this.time = time;
                this.inputs = inputs;
            }
        }
        
        List<InputsForTime> m_Inputs = new List<InputsForTime>(10);
        List<int> m_Height = new List<int>(10);
        
        static Random s_Random = new Random();
        
        public InputRecordableItem(RecordableInfo recordableInfo, int frame) : base(recordableInfo.instanceID)
        {
            m_InstanceId = recordableInfo.instanceID;
            AddToInputs(GetInputsForFrame(recordableInfo.recordable, frame));
        }

        void AddToInputs(InputsForTime inputs, int height = -1)
        {
            m_Inputs.Add(inputs);
            m_Height.Add(height);
        }

        static InputsForTime GetInputsForFrame(Recordable recordable, int frame)
        {
            var inputRec = recordable as InputRecordable;
            if (inputRec == null) return new InputsForTime();
            return new InputsForTime(GameDebuggerDatabase.GetRecords(frame).time, inputRec.inputs);
        }
        
        protected override void DrawItem(Track track, ITimeConverter converter)
        {
            var oldColor = GUI.color;
            for (int i = 0; i < m_Inputs.Count; i++)
            {
                if (m_Height[i] == -1)
                    m_Height[i] = s_Random.Next(0, 34);

                var pixel = converter.TimeToPixel(m_Inputs[i].time);
                var line = new Rect(pixel, 0, 3, 45);
                var pos = new Rect(pixel+3, m_Height[i], 50, 50);
                foreach (var input in m_Inputs[i].inputs)
                {
                    var color = StringToColor(input);
                    EditorGUI.DrawRect(line, color);
                    GUI.Label(pos, input);
                }
            }

            GUI.color = oldColor;
        }

        public override void Refresh(RecordableInfo recordableInfo, int frame)
        {
            AddToInputs(GetInputsForFrame(recordableInfo.recordable, frame));
        }

        protected override string ItemName()
        {
            return "Inputs";
        }
        
        static Color StringToColor(string str)
        {
            var hash = str.GetHashCode();
            var color =  new Color(((hash & 0xFF0000) >> 16)/255.0f, ((hash & 0x00FF00) >> 8)/255.0f, (hash & 0x0000FF)/255.0f);
            return color;
        }
    }
}
