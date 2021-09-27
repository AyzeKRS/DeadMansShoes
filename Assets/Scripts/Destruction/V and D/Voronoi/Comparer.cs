using System.Collections.Generic;
using UnityEngine;

public class Comparer : IComparer<TrianglePoints>
{
    #region Variables
    public List<Vector2> vertices;
	public List<int> triangles;
    #endregion

    #region Start compare
    public int Compare(TrianglePoints point1, TrianglePoints point2)
	{
		if (point1.point < point2.point)
			return -1;

		else if (point1.point > point2.point)
			return 1;

		else if (point1.triangle == point2.triangle)
			return 0;

		else
			return CompareAngles(point1, point2);
	}

	private int CompareAngles(TrianglePoints point1, TrianglePoints point2)
	{
		Vector2 points1 =
			vertices[triangles[point1.triangle]] / 3 +
			vertices[triangles[point1.triangle + 1]] / 3 +
			vertices[triangles[point1.triangle + 2]] / 3 -
			vertices[point1.point];

		Vector2 points2 =
			vertices[triangles[point2.triangle]] / 3 +
			vertices[triangles[point2.triangle + 1]] / 3 +
			vertices[triangles[point2.triangle + 2]] / 3 -
			vertices[point1.point];

		if (((points1.y < 0) || ((points1.y == 0) && (points1.x < 0))) ==
			((points2.y < 0) || ((points2.y == 0) && (points2.y < 0))))
		{
			if ((points1.x * points2.y - points1.y * points2.x) > 0)
				return -1;

			else if ((points1.x * points2.y - points1.y * points2.x) < 0)
				return 1;

			else
				return 0;
		}

		else
			if ((points2.y < 0) || ((points2.y == 0) && (points2.y < 0)))
			return -1;

		else
			return 1;
	}
    #endregion
}