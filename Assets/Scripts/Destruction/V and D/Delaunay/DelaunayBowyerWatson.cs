using System.Collections.Generic;
using UnityEngine;

public class DelaunayBowyerWatson
{
    #region Variables
    private IList<Vector2> vertices;
	private List<Node> triangles;

	private int highest = 0;
    #endregion

    #region Init
    public DelaunayBowyerWatson()
	{
		triangles = new List<Node>();
	}
    #endregion

    #region Calculate using BowyerWatson
    public DelaunayVariables CalculateTriangulation(IList<Vector2> _vertices, DelaunayVariables result)
	{
		vertices = _vertices;
		triangles.Clear();

		result.vertices.Clear();
		result.triangles.Clear();
		
		for (int i = 0; i < _vertices.Count; i++)
		{
			if (IsHigher(highest, i)) highest = i;
		}

		triangles.Add(new Node(new Vector3(-2, -1, highest)));

		BowyerWatsonAlgorithm();

		for (int i = 0; i < vertices.Count; i++)
			result.vertices.Add(vertices[i]);

		for (int i = 1; i < triangles.Count; i++)
		{
			Node triangle_node = triangles[i];
			if (triangle_node.InsideTriangle() && triangle_node.EndNode())
			{
				result.triangles.Add((int)triangle_node.points.x);
				result.triangles.Add((int)triangle_node.points.y);
				result.triangles.Add((int)triangle_node.points.z);
			}
		}

		return result;
	}

	private void BowyerWatsonAlgorithm()
	{
		// For every vertice
		for (int i = 0; i < vertices.Count; i++)
		{
			if (i == highest) continue;

			int count = 0;

			// Loop every node
			// check if points within triangles
			// else ignore
			while (!triangles[count].EndNode())
			{
				Node tr = triangles[count];

				if ((int)tr.next_triangles.x >= 0 &&
					CheckPointInTriangle
					(
						i,
						triangles[(int)tr.next_triangles.x])
					)
					count = (int)tr.next_triangles.x;

				else if ((int)tr.next_triangles.y >= 0 &&
					CheckPointInTriangle
					(
						i,
						triangles[(int)tr.next_triangles.y])
					)
					count = (int)tr.next_triangles.y;

				else
					count = (int)tr.next_triangles.z;
			}

			// Create new triangles
			int new_triangle_count_1 = triangles.Count;
			int new_triangle_count_2 = triangles.Count + 1;
			int new_triangle_count_3 = triangles.Count + 2;

			Node next_tri = triangles[count];

			Node adj_tri1 =
				new Node(new Vector3
				(
					i,
					next_tri.points.x,
					next_tri.points.y
				));
			
			Node adj_tri2 =
				new Node(new Vector3
				(
					i,
					next_tri.points.y,
					next_tri.points.z
				));
			
			Node adj_tri3 =
				new Node(new Vector3
				(
					i,
					next_tri.points.z,
					next_tri.points.x
				));

			next_tri.next_triangles.y		= new_triangle_count_2;
			next_tri.next_triangles.x		= new_triangle_count_1;
			next_tri.next_triangles.z		= new_triangle_count_3;

			triangles[count] = next_tri;

			adj_tri1.adjacent_triangles.x	= next_tri.adjacent_triangles.z;
			adj_tri1.adjacent_triangles.y	= new_triangle_count_2;
			adj_tri1.adjacent_triangles.z	= new_triangle_count_3;

			adj_tri2.adjacent_triangles.x	= next_tri.adjacent_triangles.x;
			adj_tri2.adjacent_triangles.y	= new_triangle_count_3;
			adj_tri2.adjacent_triangles.z	= new_triangle_count_1;

			adj_tri3.adjacent_triangles.x	= next_tri.adjacent_triangles.y;
			adj_tri3.adjacent_triangles.y	= new_triangle_count_1;
			adj_tri3.adjacent_triangles.z	= new_triangle_count_2;

			// Add adjacent triangles triangle list
			triangles.Add(adj_tri1);
			triangles.Add(adj_tri2);
			triangles.Add(adj_tri3);

			// Check adjacent triangles, flip if necessary
			if (adj_tri1.adjacent_triangles.x != -1)
				FlipEdge(i, (int)next_tri.points.x, (int)next_tri.points.y, new_triangle_count_1, (int)adj_tri1.adjacent_triangles.x);

			if (adj_tri2.adjacent_triangles.x != -1)
				FlipEdge(i, (int)next_tri.points.y, (int)next_tri.points.z, new_triangle_count_2, (int)adj_tri2.adjacent_triangles.x);

			if (adj_tri3.adjacent_triangles.x != -1)
				FlipEdge(i, (int)next_tri.points.z, (int)next_tri.points.x, new_triangle_count_3, (int)adj_tri3.adjacent_triangles.x);
		}
	}
	#endregion

	#region Flip edges
	private void FlipEdge(int point, int point1, int point2, int tri1, int tri2)
	{
		// Get end node
		while (!triangles[tri2].EndNode())
		{
			Node triangle = triangles[tri2];

			if ((int)triangle.next_triangles.x != -1 &&
				triangles[(int)triangle.next_triangles.x].TriangleOnEdge(point1, point2))
				tri2 = (int)triangle.next_triangles.x;

			else if ((int)triangle.next_triangles.y != -1 &&
				triangles[(int)triangle.next_triangles.y].TriangleOnEdge(point1, point2))
				tri2 = (int)triangle.next_triangles.y;

			else if ((int)triangle.next_triangles.z != -1 &&
				triangles[(int)triangle.next_triangles.z].TriangleOnEdge(point1, point2))
				tri2 = (int)triangle.next_triangles.z;
		}

		Node triangle1 = triangles[tri1];
		Node triangle2 = triangles[tri2];

		int i = (int)triangle2.GetThirdPoint(point1, point2);

		// Check edge, flip point
		if (!CheckEdge(point, i, point1, point2))
		{
			int tri_point1 = triangles.Count;
			int tri_point2 = triangles.Count + 1;

			Node next_tri1 = triangles[tri1];
			Node next_tri2 = triangles[tri2];

			Node triangle3 = new Node(new Vector3(point, point1, i));
			Node triangle4 = new Node(new Vector3(point, i, point2));

			triangle3.adjacent_triangles.x = triangle2.GetAdjacentTriangle(point2);
			triangle3.adjacent_triangles.y = tri_point2;
			triangle3.adjacent_triangles.z = triangle1.GetAdjacentTriangle(point2);

			triangle4.adjacent_triangles.x = triangle2.GetAdjacentTriangle(point1);
			triangle4.adjacent_triangles.y = triangle1.GetAdjacentTriangle(point1);
			triangle4.adjacent_triangles.z = tri_point1;

			triangles.Add(triangle3);
			triangles.Add(triangle4);

			next_tri1.next_triangles.x = tri_point1;
			next_tri1.next_triangles.y = tri_point2;

			next_tri2.next_triangles.x = tri_point1;
			next_tri2.next_triangles.y = tri_point2;

			triangles[tri1] = next_tri1;
			triangles[tri2] = next_tri2;

			// Check other triangles
			if (triangle3.adjacent_triangles.x != -1)
				FlipEdge(point, point1, i, tri_point1, (int)triangle3.adjacent_triangles.x);

			if (triangle4.adjacent_triangles.x != -1)
				FlipEdge(point, i, point2, tri_point2, (int)triangle4.adjacent_triangles.x);
		}
	}

	private bool CheckEdge(int point, int i, int point1, int point2)
	{
		if (i < 0) return true;

		else if (point1 < 0)
			return
				(
				(vertices[point2].x - vertices[point].x) *
				(vertices[i].y		- vertices[point].y) -
				(vertices[point2].y - vertices[point].y) *
				(vertices[i].x		- vertices[point].x)
				) >= 0;

		else if (point2 < 0)
			return
				(
				(vertices[point1].x - vertices[point].x) *
				(vertices[i].y		- vertices[point].y) -
				(vertices[point1].y - vertices[point].y) *
				(vertices[i].x		- vertices[point].x)
				) <= 0;

		else
		{
			Vector2 vect1 = vertices[point] - vertices[i];
			Vector2 vect2 = vertices[point1] - vertices[i];
			Vector2 vect3 = vertices[point2] - vertices[i];

			return
				(
				(vect1.x * vect1.x + vect1.y * vect1.y) * (vect2.x * vect3.y - vect3.x * vect2.y) -
				(vect2.x * vect2.x + vect2.y * vect2.y) * (vect1.x * vect3.y - vect3.x * vect1.y) +
				(vect3.x * vect3.x + vect3.y * vect3.y) * (vect1.x * vect2.y - vect2.x * vect1.y)
				) < 0.000001f;
		}
	}
    #endregion

    #region Additional Functions
    private bool CheckPointInTriangle(int point, Node triangle_node)
	{
		return
			CheckPointToLeft(point, (int)triangle_node.points.x, (int)triangle_node.points.y) &&
			CheckPointToLeft(point, (int)triangle_node.points.y, (int)triangle_node.points.z) &&
			CheckPointToLeft(point, (int)triangle_node.points.z, (int)triangle_node.points.x);
	}

	private bool CheckPointToLeft(int point, int point1, int point2)
	{
		switch (point1)
        {
			case -2:
				return IsHigher(point2, point);

			case -1:
				return IsHigher(point, point2);
        }

		switch (point2)
        {
			case -2:
				return IsHigher(point, point1);

			case -1:
				return IsHigher(point1, point);
		}
		
		return
			(
			(vertices[point2].x - vertices[point1].x) *
			(vertices[point].y	- vertices[point1].y) -
			(vertices[point2].y - vertices[point1].y) *
			(vertices[point].x	- vertices[point1].x)
			) >= 0;
	}

	private bool IsHigher(int num1, int num2)
	{
		switch (num1)
		{
			case -2:
				return false;

			case -1:
				return true;
		}

		switch (num2)
		{
			case -2:
				return true;

			case -1:
				return false;

			default:
				Vector2 point1 = vertices[num1];
				Vector2 point2 = vertices[num2];

				if (point1.y < point2.y)		return true;
				else if (point1.y > point2.y)	return false;
				else							return point1.x < point2.x;
		};
	}
    #endregion
}