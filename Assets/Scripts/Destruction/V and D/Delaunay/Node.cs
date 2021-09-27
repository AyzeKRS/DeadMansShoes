using UnityEngine;

public struct Node
{
    #region Variables
    public Vector3 points;
	public Vector3 next_triangles;
	public Vector3 adjacent_triangles;
	#endregion

	#region Init
	public Node(Vector3 _points)
	{
		points				= _points;
		next_triangles		= new Vector3(-1, -1, -1);
		adjacent_triangles	= new Vector3(-1, -1, -1);
	}
    #endregion

    #region Get next/check point
	public bool InsideTriangle()
	{
		return points.x >= 0 && points.y >= 0 && points.z >= 0;
	}

    public bool TriangleOnEdge(int point1, int point2)
    {
		if (point1 == points.x)
			return point2 == points.y || point2 == points.z;

		if (point1 == points.y)
			return point2 == points.x || point2 == points.z;

		if (point1 == points.z)
			return point2 == points.x || point2 == points.y;

		return false;
    }
    
    public float GetThirdPoint(int point1, int point2)
	{
		if (point1 == points.x)
		{
			if (point2 == points.y) return points.z;
			if (point2 == points.z) return points.y;
		}

		if (point1 == points.y)
		{
			if (point2 == points.x) return points.z;
			if (point2 == points.z) return points.x;
		}

		if (point1 == points.z)
		{
			if (point2 == points.x) return points.y;
			if (point2 == points.y) return points.x;
		}

		return 0.0f;
	}
	#endregion

	#region Get next/check triangle
	public bool EndNode()
	{
		return next_triangles.x < 0 && next_triangles.y < 0 && next_triangles.z < 0;
	}

	public float GetAdjacentTriangle(int point)
	{
		if (point == points.x) return adjacent_triangles.x;
		if (point == points.y) return adjacent_triangles.y;
		if (point == points.z) return adjacent_triangles.z;

		return 0;
	}
    #endregion
}