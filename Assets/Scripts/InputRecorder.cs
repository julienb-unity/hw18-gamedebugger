using System.Collections.Generic;
using UnityEngine;

namespace GameDebugger
{
    public class InputRecorder : MonoBehaviour
    {
        List<string> currentInputs = new List<string>(5);

        void Update()
        {
            foreach (var key in s_Keys)
            {
                if (Input.GetKeyDown(key))
                    currentInputs.Add(key);
            }
        }

        public void FlushInputs()
        {
            currentInputs.Clear();;
        }

        public IEnumerable<string> inputs
        { 
            get
            {
                return currentInputs;
            }
        }
        
        static string[] s_Keys =
            {
                "backspace",
                "delete",
                "tab",
                "clear",
                "return",
                "pause",
                "escape",
                "space",
                "up",
                "down",
                "right",
                "left",
                "insert",
                "home",
                "end",
                "page up",
                "page down",
                "f1",
                "f2",
                "f3",
                "f4",
                "f5",
                "f6",
                "f7",
                "f8",
                "f9",
                "f10",
                "f11",
                "f12",
                "f13",
                "f14",
                "f15",
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "!",
                "\"",
                "#",
                "$",
                "&",
                "'",
                "(",
                ")",
                "*",
                "+",
                ",",
                "-",
                ".",
                "/",
                ":",
                ";",
                "<",
                "=",
                ">",
                "?",
                "@",
                "[",
                "\\",
                "]",
                "^",
                "_",
                "`",
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
                "l",
                "m",
                "n",
                "o",
                "p",
                "q",
                "r",
                "s",
                "t",
                "u",
                "v",
                "w",
                "x",
                "y",
                "z",
                "numlock",
                "caps lock",
                "scroll lock",
                "right shift",
                "left shift",
                "right ctrl",
                "left ctrl",
                "right alt",
                "left alt"
            };
    }
}
