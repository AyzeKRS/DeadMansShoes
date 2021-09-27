using System.Collections.Generic;
using UnityEngine;

public class MeshDestruction : MonoBehaviour
{
    #region Variables
    #region Breaking
	public float						break_force			= 0.0f;
	public int							shatter_amount		= 0;
	[SerializeField] private bool		break_again			= true;
	[SerializeField] private bool		throw_enabled		= false;
	[SerializeField] private Vector3	throw_amount		= Vector3.zero;

	#endregion

	#region Poly variables
	public List<Vector2> poly;
	public float z_width			= 1.0f;
	private float area				= -1.0f;
    #endregion

    #region Mesh
        private MeshCollider c;
		private MeshFilter f;
        #endregion
    #endregion

    #region BuiltIn Functions
    private void Start()
	{
		Init();
	}

	private void Update()
	{
		// Break wall with button press (LEGACY: Was for testing purposes only)
		//if (Input.GetKeyDown(KeyCode.Alpha1) && break_via_button)
		//	BreakPoly(transform.InverseTransformPoint(transform.position));
	}

	private void OnCollisionEnter(Collision c)
	{
		if (c.impactForceSum.magnitude > break_force && break_again)
        {
			Audio.Instance.Play3DAway("Breaking", gameObject);
			BreakMesh(transform.InverseTransformPoint(c.contacts[0].point));
		}
			
	}
    #endregion

    #region Custom Functions
    private void Init()
	{
		c	= GetComponent<MeshCollider>();
		f	= GetComponent<MeshFilter>();

		CreatePoly();
	}

	private void CreatePoly()
    {
		if (poly.Count == 0)
		{
			poly.Add(new Vector2
				(
				-transform.localScale.x / 2.0f,
				-transform.localScale.y / 2.0f
				));

			poly.Add(new Vector2
				(
				transform.localScale.x / 2.0f,
				-transform.localScale.y / 2.0f
				));

			poly.Add(new Vector2
				(
				transform.localScale.x / 2.0f,
				transform.localScale.y / 2.0f
				));

			poly.Add(new Vector2
				(
				-transform.localScale.x / 2.0f,
				transform.localScale.y / 2.0f
				));

			z_width = transform.localScale.z;
			transform.localScale = Vector3.one;
		}

		Mesh mesh = CreateMeshFromPolygon(poly, z_width);

		c.sharedMesh = mesh;
		f.sharedMesh = mesh;
	}

	private void BreakMesh(Vector2 position)
	{
		VoronoiDiagram diagram		= new VoronoiDiagram();
		VoronoiDivider divide		= new VoronoiDivider();
		
		Vector2[] new_poly_counts	= new Vector2[shatter_amount];

		if (area < 0.0f)
		{
			area = 0;
			for (int i = 0; i < poly.Count; i++)
			{
				int j = (i != (poly.Count - 1)) ? i + 1 : 0;

				area += poly[i].x * poly[j].y - poly[j].y * poly[j].x;
			}
		}

		for (int i = 0; i < new_poly_counts.Length; i++)
		{
			float random_normal =
			Mathf.Sqrt(-2.0f * Mathf.Log(Random.value)) *
			Mathf.Sin(2.0f * Mathf.PI * Random.value);

			float dist	= Mathf.Abs((0.5f * random_normal) + 0.5f);
			float angle	= 2.0f * Mathf.PI * Random.value;

			new_poly_counts[i] = position +
				new Vector2 (dist * Mathf.Cos(angle), dist * Mathf.Sin(angle));
		}

		VoronoiVariables variables = diagram.CalculateDiagram(new_poly_counts);

		List<Vector2> divided = new List<Vector2>();

		for (int i = 0; i < new_poly_counts.Length; i++)
		{
			divided = divide.DivideRegion(variables, poly, i, divided);

			if (divided.Count > 0)
			{
				GameObject go = Instantiate(gameObject, transform.parent);

				go.transform.localPosition = transform.localPosition;
				go.transform.localRotation = transform.localRotation;
				go.name = name + " " + i.ToString();

				if (throw_enabled)
				{
					go.GetComponent<Rigidbody>().AddForce(throw_amount);
				}

				MeshDestruction d = go.GetComponent<MeshDestruction>();

				d.break_again = false;
				d.poly.Clear();
				d.poly.AddRange(divided);
			}
		}

		Destroy(gameObject);
	}

	private static Mesh CreateMeshFromPolygon(List<Vector2> polys, float z_width)
	{
		int poly_count		= polys.Count;
		
		Vector3[] vertices	= new Vector3[poly_count * 6];
		int vertice_count	= 0;

		Vector3[] normals	= new Vector3[poly_count * 6];
		int normal_count	= 0;

		int[] triangles		= new int[3 * (4 * poly_count - 4)];
		int triangles_count = 0;

		// Top
		for (int i = 0; i < poly_count; i++)
		{
			vertices[vertice_count++] =
				new Vector3(polys[i].x, polys[i].y, z_width * 0.5f);

			normals[normal_count++] =
				Vector3.forward;
		}

		// Bottom
		for (int i = 0; i < poly_count; i++)
		{
			vertices[vertice_count++] =
				new Vector3(polys[i].x, polys[i].y, -(z_width / 2.0f));
			normals[normal_count++] = Vector3.back;
		}

		// Sides
		for (int i = 0; i < poly_count; i++)
		{
			int j = (i != (poly_count - 1)) ? i + 1 : 0;

			vertices[vertice_count++] =
				new Vector3(polys[i].x, polys[i].y, z_width / 2.0f);

			vertices[vertice_count++] =
				new Vector3(polys[i].x, polys[i].y, -(z_width / 2.0f));

			vertices[vertice_count++] =
				new Vector3(polys[j].x,	polys[j].y, -(z_width / 2.0f));

			vertices[vertice_count++] =
				new Vector3(polys[j].x, polys[j].y,	z_width / 2.0f);

			Vector3 _normals =
				Vector3.Cross(polys[j] - polys[i], Vector3.forward).normalized;

			normals[normal_count++] = _normals;
			normals[normal_count++] = _normals;
			normals[normal_count++] = _normals;
			normals[normal_count++] = _normals;
		}

		for (int i = 2; i < poly_count; i++)
		{
			triangles[triangles_count++] = 0;
			triangles[triangles_count++] = i - 1;
			triangles[triangles_count++] = i;
		}

		for (int i = 2; i < poly_count; i++)
		{
			triangles[triangles_count++] = poly_count;
			triangles[triangles_count++] = poly_count + i;
			triangles[triangles_count++] = poly_count + i - 1;
		}

		for (int i = 0; i < poly_count; i++)
		{
			triangles[triangles_count++] = (2 * poly_count) + (4 * i);
			triangles[triangles_count++] = (2 * poly_count) + (4 * i) + 1;
			triangles[triangles_count++] = (2 * poly_count) + (4 * i) + 2;
					 					   
			triangles[triangles_count++] = (2 * poly_count) + (4 * i);
			triangles[triangles_count++] = (2 * poly_count) + (4 * i) + 2;
			triangles[triangles_count++] = (2 * poly_count) + (4 * i) + 3;
		}

		Mesh mesh = new Mesh();

		mesh.vertices	= vertices;
		mesh.normals	= normals;
		mesh.triangles	= triangles;
		
		return mesh;
	}
	#endregion
}

