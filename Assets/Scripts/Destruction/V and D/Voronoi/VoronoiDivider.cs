using System.Collections.Generic;
using UnityEngine;

public class VoronoiDivider
{
    #region Variables
    private List<Vector2> points_in		= new List<Vector2>();
	private List<Vector2> points_out	= new List<Vector2>();
    #endregion

    #region Init
    public VoronoiDivider() { }
    #endregion

    #region Divide shape
    public List<Vector2> DivideRegion(VoronoiVariables diagram, IList<Vector2> polygon, int region, List<Vector2> divide)
	{
		int first_edge = 0;
		int last_edge  = 0;

		points_in.Clear();
		points_in.AddRange(polygon);
		
		if (region == diagram.regions.Count - 1)
		{
			first_edge	= diagram.first_edge[region];
			last_edge	= diagram.edges.Count - 1;
		}
		
		else
		{
			first_edge	= diagram.first_edge[region];
			last_edge	= diagram.first_edge[region + 1] - 1;
		}

		for (int i = first_edge; i <= last_edge; i++)
		{
			points_out.Clear();

			Edge edge	= diagram.edges[i];

			Vector2 lp	= Vector2.zero;
			Vector2 ld	= Vector2.zero;

			switch (edge.edge)
			{
				case RegionEdge.LINE:
					lp = diagram.vertices[edge.vertice1];
					ld = diagram.vertices[edge.vertice2] - diagram.vertices[edge.vertice1];
					break;

				case RegionEdge.CLOCKWISE:
					lp = diagram.vertices[edge.vertice1];
					ld = -(edge.dir);
					break;

				case RegionEdge.COUNTERCLOCKWISE:
					lp = diagram.vertices[edge.vertice1];
					ld = edge.dir;
					break;

				default:
					break;
			}

			for (int j = 0; j < points_in.Count; j++)
			{
				int k = (j == points_in.Count - 1) ? 0 : j + 1;

				Vector2 point1 = points_in[j];
				Vector2 point2 = points_in[k];

				bool check_point1 =
					((
					(lp + ld).x - lp.x) *
					(point1.y - lp.y) -
					(
					(lp + ld).y - lp.y) *
					(point1.x - lp.x)
					) >= 0;

				bool check_point2 =
					((
					(lp + ld).x - lp.x) *
					(point2.y - lp.y) -
					((lp + ld).y - lp.y) *
					(point2.x - lp.x)
					) >= 0;

				if (check_point1 && check_point2)
					points_out.Add(point2);
				
				else if
					(
					(check_point1 && !check_point2) ||
					(!check_point1 && check_point2)
					)
				{
					Vector2 intersection = lp + ((
							(lp.y - point1.y) * (point2 - point1).normalized.x -
							(lp.x - point1.x) * (point2 - point1).normalized.y) /
							(ld.normalized.x * (point2 - point1).normalized.y -
							ld.normalized.y * (point2 - point1).normalized.x) *
							ld.normalized
							);
							
					if (check_point1)
						points_out.Add(intersection);

					else if (check_point2)
					{
						points_out.Add(intersection);
						points_out.Add(point2);
					}
				}
			}

			List<Vector2> swap_point = points_out;
			points_out	= points_in;
			points_in	= swap_point;
		}

		divide.Clear();
		divide.AddRange(points_in);

		return divide;
	}
    #endregion
}

