using System.Collections.Generic;
using UnityEngine;

public class VoronoiVariables
{
    #region Variables
	public List<Vector2> vertices;
	public List<Edge> edges;
	public List<int> first_edge;

	public DelaunayVariables delaunay_triangles;
	public List<Vector2> regions;
	#endregion

	#region Init
	public VoronoiVariables()
	{
		vertices			= new List<Vector2>();
		edges				= new List<Edge>();
		first_edge			= new List<int>();

		delaunay_triangles	= new DelaunayVariables();
		regions				= delaunay_triangles.vertices;
	}
    #endregion
}

