using System.Collections.Generic;
using UnityEngine;

public class DelaunayVariables
{
    #region Variables
    public List<Vector2> vertices;
	public List<int> triangles;
    #endregion

    #region Init
    public DelaunayVariables()
	{
		vertices	= new List<Vector2>();
		triangles	= new List<int>();
	}
    #endregion
}

