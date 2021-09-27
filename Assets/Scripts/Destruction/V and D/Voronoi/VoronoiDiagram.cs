using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram
{
    #region Variables
	List<TrianglePoints> points;
	Comparer compare;
    #endregion

    #region Init
    public VoronoiDiagram()
	{
		points	= new List<TrianglePoints>();
		compare = new Comparer();
	}
    #endregion

    #region Create diagram
    public VoronoiVariables CalculateDiagram(IList<Vector2> input_vertices)
    {
		VoronoiVariables result = new VoronoiVariables();

		result.regions.Clear();
		result.vertices.Clear();
		result.edges.Clear();
		result.first_edge.Clear();

		result.delaunay_triangles.vertices.Clear();
		result.delaunay_triangles.triangles.Clear();

		points.Clear();

		DelaunayVariables del_vars	= result.delaunay_triangles;
		DelaunayBowyerWatson bw		= new DelaunayBowyerWatson();

		del_vars = bw.CalculateTriangulation(input_vertices, del_vars);

		List<Vector2> vertices	= del_vars.vertices;
		List<int> triangles		= del_vars.triangles;

		List<Vector2> centres	= result.vertices;
		List<Edge> edges		= result.edges;

		if (triangles.Count > points.Capacity)	{ points.Capacity	= triangles.Count;	}
		if (triangles.Count > edges.Capacity)	{ edges.Capacity	= triangles.Count;	}

		for (int i = 0; i < triangles.Count; i += 3)
		{
			Vector2 vertice1	= vertices[triangles[i]] - vertices[triangles[i + 1]];
			vertice1			= new Vector2(-vertice1.y, vertice1.x);

			Vector2 vertice2	= vertices[triangles[i + 1]] - vertices[triangles[i + 2]];
			vertice2			= new Vector2(-vertice2.y, vertice2.x);

			Vector2 point1 = (vertices[triangles[i]] + vertices[triangles[i + 1]]) / 2.0f;
			Vector2 point2 = (vertices[triangles[i + 1]] + vertices[triangles[i + 2]]) / 2.0f;

			centres.Add
				(
				point1 + (
				(point1.y - point2.y) * vertice2.x -
				(point1.x - point2.x) * vertice2.y) /
				(vertice1.x * vertice2.y - vertice1.y * vertice2.x) *
				vertice1
				);

			points.Add(new TrianglePoints(triangles[i], i));
			points.Add(new TrianglePoints(triangles[i + 1], i));
			points.Add(new TrianglePoints(triangles[i + 2], i));
		}

		compare.triangles	= triangles;
		compare.vertices	= vertices;
		points.Sort(compare);

		for (int i = 0; i < points.Count; i++)
		{
			result.first_edge.Add(edges.Count);

			int start	= i;
			int end		= -1;

			for (int j = i + 1; j < points.Count; j++)
				if (points[i].point != points[j].point)
				{
					end = j - 1;
					break;
				}

			if (end == -1)
				end = points.Count - 1;

			for (int j = start; j <= end; j++)
			{
				int k = j + 1;

				if (k > end)
					k = start;

				TrianglePoints current_point	= points[j];
				TrianglePoints next_point		= points[k];

				int current_tri		= current_point.triangle;
				int next_tri		= next_point.triangle;
				bool edge_triangle	= false;

				switch (end - start)
				{
					case 0:
						edge_triangle = true;
						break;

					case 1:
						Vector2 current =
							vertices[triangles[current_tri]] / 3 +
							vertices[triangles[current_tri + 1]] / 3 +
							vertices[triangles[current_tri + 2]] / 3;

						Vector2 next =
							vertices[triangles[next_tri]] / 3 +
							vertices[triangles[next_tri + 1]] / 3 +
							vertices[triangles[next_tri + 2]] / 3;

						edge_triangle = (
							(next.x - vertices[current_point.point].x) * (current.y - vertices[current_point.point].y) -
							(next.y - vertices[current_point.point].y) * (current.x - vertices[current_point.point].x)
							) >= 0;

						break;

					default:
						Vector3 current_tris =
							new Vector3(triangles[current_tri], triangles[current_tri + 1], triangles[current_tri + 2]);

						Vector3 next_tris =
							new Vector3(triangles[next_tri], triangles[next_tri + 1], triangles[next_tri + 2]);

						int n = 0;

						if (current_tris.x == next_tris.x || current_tris.x == next_tris.y || current_tris.x == next_tris.z) n++;
						if (current_tris.y == next_tris.x || current_tris.y == next_tris.y || current_tris.y == next_tris.z) n++;
						if (current_tris.z == next_tris.x || current_tris.z == next_tris.y || current_tris.z == next_tris.z) n++;

						edge_triangle = n < 2;
						break;
				}

				if (edge_triangle)
				{
					Vector2 vertice1 = Vector2.zero;

					if (current_point.point == triangles[current_tri])
						vertice1 = vertices[triangles[current_tri + 2]] - vertices[triangles[current_tri + 0]];

					else if (current_point.point == triangles[current_tri + 1])
						vertice1 = vertices[triangles[current_tri + 0]] - vertices[triangles[current_tri + 1]];

					else
						vertice1 = vertices[triangles[current_tri + 1]] - vertices[triangles[current_tri + 2]];

					edges.Add(new Edge
						(
						current_point.point,
						current_tri / 3,
						-1,
						new Vector2(-vertice1.y, vertice1.x),
						RegionEdge.COUNTERCLOCKWISE
					));

					Vector2 vertice2 = Vector2.zero;

					if (next_point.point == triangles[next_tri])
						vertice2 = vertices[triangles[next_tri + 0]] - vertices[triangles[next_tri + 1]];

					else if (next_point.point == triangles[next_tri + 1])
						vertice2 = vertices[triangles[next_tri + 1]] - vertices[triangles[next_tri + 2]];

					else
						vertice2 = vertices[triangles[next_tri + 2]] - vertices[triangles[next_tri + 0]];

					edges.Add(new Edge
						(
						current_point.point,
						next_tri / 3,
						-1,
						new Vector2(-vertice2.y, vertice2.x),
						RegionEdge.CLOCKWISE
					));
				}

				else
				{
					if (((centres[current_tri / 3] - centres[next_tri / 3]).magnitude > 0.000001f))
					{
						edges.Add(new Edge(
							current_point.point,
							current_tri / 3,
							next_tri / 3,
							Vector2.zero,
							RegionEdge.LINE
						));
					}
				}
			}
			i = end;
		}

		return result;
	}
    #endregion
}

