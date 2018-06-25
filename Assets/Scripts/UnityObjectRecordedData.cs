using System.Collections.Generic;

public class UnityObjectRecordedData
{
	public int instanceId;
	public List<PropertyData> propertyData = new List<PropertyData>();
		
	public UnityObjectRecordedData(int instanceId)
	{
		this.instanceId = instanceId;
	}

	public PropertyData GetPropertyData(string propertyName)
	{
		foreach (var propData in propertyData)
		{
			if (propData.fieldName == propertyName) return propData;
		}
		return null;
	}
}