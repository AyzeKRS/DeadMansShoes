using UnityEngine;

#region Enums
public enum RegionEdge
{
	LINE,					// Normal line
	CLOCKWISE,				// Line moving clockwise
	COUNTERCLOCKWISE		// Line moving counter clockwise
}
#endregion

public struct Edge
{
	#region Variables
	public int region;
	public int vertice1;
	public int vertice2;
	public Vector2 dir;
	public RegionEdge edge;
    #endregion

    #region Init
    public Edge(int _region, int _vertice1, int _vertice2, Vector2 _dir, RegionEdge _edge)
	{
		region		= _region;
		vertice1	= _vertice1;
		vertice2	= _vertice2;
		dir			= _dir;
		edge		= _edge;
	}
    #endregion
}