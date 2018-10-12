using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshInverter : MonoBehaviour {

	private Mesh mesh;

	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.triangles = mesh.triangles.Reverse().ToArray();
	}
}
