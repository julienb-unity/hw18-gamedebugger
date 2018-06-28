

using System.Collections;
using System.IO;
using UnityEngine;

namespace Recordables
{
	public class ScreenShotRecordable : Recordable<RecordingFilter>, ISerializationCallbackReceiver
	{
		[SerializeField] private string path;
		public Texture2D tex { get; private set; }

        IEnumerator RecordFrame()
        {
            yield return new WaitForEndOfFrame();
            tex = ScreenCapture.CaptureScreenshotAsTexture();
        }
        public override bool OnRecord(Recordable previous, Object source)
		{
            if (GameDebuggerRecorder.currentFrame % 60 == 0)
            {
                var s = source as MonoBehaviour;
                if (s != null)
                {
                    path = string.Format("{0}/../capture_{1}.png", Application.dataPath, GameDebuggerRecorder.currentFrame);
                    s.StartCoroutine(RecordFrame());
                }
                return true;
			}

			return false;
		}

		public void OnBeforeSerialize()
		{
			if (tex == null)
				return;
			
			var bytes = tex.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
		}

		public void OnAfterDeserialize()
		{
		}
	}
}
