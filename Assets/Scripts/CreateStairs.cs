using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Handout {
	public class CreateStairs : MonoBehaviour {
		public int numberOfSteps = 10;
		// The dimensions of a single step of the staircase:
		public float width=3;
		public float height=1;
		public float depth=1;
		MeshBuilder builder;
		public float angle;
		private Vector3 normalDepth = new Vector3(0,0,1);
		private Vector3 normalWidth = new Vector3(1,0,0);
		private Vector3 offset = new Vector3(0,0,0);
		private float shortDepth	;

		void Start () {
			builder = new MeshBuilder ();
			CreateShape ();
			GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
			
		}

		/// <summary>
		/// Creates a stairway shape in [builder].
		/// </summary>
		void CreateShape() {
			builder.Clear ();

			shortDepth = depth - Mathf.Sqrt((2 * (width * width)) - (2 * width * width * Mathf.Cos(angle * Mathf.Deg2Rad)));
			

            for (int i = 0; i < numberOfSteps; i++) {
				 
				

				// TODO 1: use the width and height parameters from the inspector to change the step width and height

				// TODO 4: Fix the uvs:
				// bottom:
				int v1 = builder.AddVertex (offset + width * normalWidth, new Vector2 (0, 0));	
				int v2 = builder.AddVertex (offset, new Vector2 (1, 0));
				// top front:
				int v3 = builder.AddVertex (offset + width * normalWidth + new Vector3(0, height, 0), new Vector2 (0, 0.5f));
				int v4 = builder.AddVertex (offset + new Vector3(0, height, 0), new Vector2(1, 0.5f));
				// top back:
				int v5 = builder.AddVertex (offset + width * normalWidth + new Vector3(0, height, 0) + normalDepth * shortDepth, new Vector2 (0, 1));	
				int v6 = builder.AddVertex (offset + new Vector3(0, height, 0) + normalDepth * depth, new Vector2 (1, 1));

				// TODO 2: Fix the winding order (everything clockwise):
				builder.AddTriangle (v1, v2, v3);
				builder.AddTriangle (v4, v6, v5);

                v2 = builder.AddVertex(offset, new Vector2(1, 0));
                // top front:
                v3 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0), new Vector2(0, 0.5f));
                v4 = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(1, 0.5f));
                // top back:


                builder.AddTriangle(v2, v4, v3);

                v3 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0), new Vector2(0, 0.5f));
                v4 = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(1, 0.5f));
                v5 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0) + normalDepth * shortDepth, new Vector2(0, 1));

                builder.AddTriangle(v3, v4, v5);

                // Back

                v1 = builder.AddVertex(offset + width * normalWidth, new Vector2(0, 0));
                v2 = builder.AddVertex(offset, new Vector2(1, 0));
                v5 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0) + normalDepth * shortDepth, new Vector2(0, 1));

                builder.AddTriangle(v1, v5, v2);

                v2 = builder.AddVertex(offset, new Vector2(1, 0));
                v5 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0) + normalDepth * shortDepth, new Vector2(0, 1));
                v6 = builder.AddVertex(offset + new Vector3(0, height, 0) + normalDepth * depth, new Vector2(1, 1));
                builder.AddTriangle(v2, v5, v6);


                // bottom:
                v1 = builder.AddVertex(offset + width * normalWidth, new Vector2(0, 0));
                v2 = builder.AddVertex(offset, new Vector2(1, 0));
                // top front:
                v3 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0), new Vector2(0, 1));
                v4 = builder.AddVertex(offset + new Vector3(0, height, 0), new Vector2(1, 1));
                // top back:
                v5 = builder.AddVertex(offset + width * normalWidth + new Vector3(0, height, 0) + normalDepth * shortDepth, new Vector2(1, 1));
                v6 = builder.AddVertex(offset + new Vector3(0, height, 0) + normalDepth * depth, new Vector2(0, 1));

                // TODO 3: make the mesh solid by adding left, right and back side.
                // Left
                builder.AddTriangle(v2, v6, v4);
				
				// Right
				builder.AddTriangle(v1, v3, v5);
                normalDepth = normalDepth = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * normalDepth.x - Mathf.Sin(Mathf.Deg2Rad * angle) * normalDepth.z, normalDepth.y,
					Mathf.Sin(Mathf.Deg2Rad * angle) * normalDepth.x + Mathf.Cos(Mathf.Deg2Rad * angle) * normalDepth.z);
                normalWidth = normalWidth = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * normalWidth.x - Mathf.Sin(Mathf.Deg2Rad * angle) * normalWidth.z, normalDepth.y,
                    Mathf.Sin(Mathf.Deg2Rad * angle) * normalWidth.x + Mathf.Cos(Mathf.Deg2Rad * angle) * normalWidth.z);


                offset = builder.GetVertex(v6);
				
                // TODO 5: Fix the normals by *not* reusing a single vertex in multiple triangles with different normals (solve it by creating more vertices at the same position)
            }
			
		}
		
	}
}