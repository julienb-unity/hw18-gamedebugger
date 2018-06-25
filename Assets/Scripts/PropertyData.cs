using System.Collections.Generic;

public class PropertyData
{
	public int firstFrameId;
	public string fieldName;
	public List<object> frameData = new List<object>();

	public PropertyData(int firstFrame)
	{
		firstFrameId = firstFrame;
	}
}