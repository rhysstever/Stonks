using System.Collections;
using System.Collections.Generic;

public class UpgradeTier<T>
{
	#region Fields
	private string name;
	private T data;
	private float cost;
	private UpgradeTier<T> nextTier;
	#endregion

	#region Properties
	public T Data { get { return data; } }
	public float Cost { get { return cost; } }
	public UpgradeTier<T> Next { get { return nextTier; } }
	#endregion

	#region Constructor
	public UpgradeTier(string name, T data, float cost, UpgradeTier<T> nextTier)
	{
		this.name = name;
		this.data = data;
		this.cost = cost;
		this.nextTier = nextTier;
	}
	#endregion

	#region Methods
	
	#endregion
}
