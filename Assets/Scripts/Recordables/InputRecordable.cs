﻿using System;
using System.Collections.Generic;
using GameDebugger;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
    [Serializable]
    [UsedImplicitly]
    public class InputRecordable : Recordable<InputRecorder>
    {
        [SerializeField]
        List<string> m_Inputs = new List<string>();

        public List<string> inputs
        {
            get { return m_Inputs; }
        }

        public override bool OnRecord(Recordable previous, Object source)
        {
            var recorder = source as InputRecorder;
            if (recorder != null)
            {
                m_Inputs.AddRange(recorder.inputs);
                recorder.FlushInputs();
            }

            return true;
        }

        public override void OnReplay(Object source)
        {}
    }
}


