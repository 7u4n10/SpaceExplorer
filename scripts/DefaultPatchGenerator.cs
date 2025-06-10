using Godot;

public class DefaultPatchGenerator : IPatchMeshGenerator
{
	// version 3
	public ArrayMesh GeneratePatchMesh(
		Vector3 localUp,
		Vector3 axisA,
		Vector3 axisB,
		Vector2 offset,
		float scale,
		float radius,
		TerrainNoiseProfile profile,
		int resolution
	)
	{
		// DEBUG
		//GD.Print("[MeshGenerator] GeneratePatchMesh() called");

		var st = new SurfaceTool();
		st.Begin(Mesh.PrimitiveType.Triangles);

		int vertexResolution = resolution + 1;

		for (int y = 0; y < vertexResolution; y++)
		{
			for (int x = 0; x < vertexResolution; x++)
			{
				float u = (x / (float)resolution) * scale + offset.X;
				float v = (y / (float)resolution) * scale + offset.Y;

				Vector3 pointOnUnitCube = localUp
					+ (u - 0.5f) * 2f * axisA
					+ (v - 0.5f) * 2f * axisB;

				Vector3 pointOnSphere = pointOnUnitCube.Normalized();
				float elevation = profile.GetFractalElevation(pointOnSphere);

				Vector3 vertex = pointOnSphere * (radius + elevation);

				st.SetNormal(pointOnSphere);
				st.AddVertex(vertex);
			}
		}

		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				int i = x + y * vertexResolution;

				st.AddIndex(i);
				st.AddIndex(i + vertexResolution);
				st.AddIndex(i + vertexResolution + 1);

				st.AddIndex(i);
				st.AddIndex(i + vertexResolution + 1);
				st.AddIndex(i + 1);
			}
		}

		// DEBUG
		if (resolution <= 0 || vertexResolution <= 1)
		{
			GD.PrintErr("[MeshGenerator] ERROR: Invalid resolution — no vertices will be created.");
			return null;
		}

		ArrayMesh mesh = st.Commit();
		
		// DEBUG
		if (mesh == null)
		{
			GD.PrintErr("[MeshGenerator] Mesh is null!");
			return null;
		}

		var aabb = mesh.GetAabb();
		//GD.Print($"[MeshGenerator] Mesh AABB Position: {aabb.Position}, Size: {aabb.Size}");

		if (aabb.Size.Length() == 0)
		{
			GD.PrintErr("[MeshGenerator] WARNING: Mesh AABB has zero size. Mesh may be invisible.");
		}

		return mesh;
	}
	
	// version 2
	//private float GetFractalElevation(Vector3 point, FastNoiseLite noise)
	//{
		//float elevation = 0f;
		//float frequency = 1f;
		//float amplitude = 1f;
//
		//for (int i = 0; i < 4; i++)
		//{
			//elevation += noise.GetNoise3D(
				//point.X * frequency,
				//point.Y * frequency,
				//point.Z * frequency
			//) * amplitude;
//
			//amplitude *= 0.5f;
			//frequency *= 2f;
		//}
//
		//return elevation;
	//}
	
	// version 1
	//public ArrayMesh GeneratePatchMesh(
		//Vector3 localUp,
		//Vector3 axisA,
		//Vector3 axisB,
		//Vector2 offset,
		//float scale,
		//float radius,
		//FastNoiseLite noise,
		//float elevationAmplitude,
		//int resolution
	//)
	//{
		//var st = new SurfaceTool();
		//st.Begin(Mesh.PrimitiveType.Triangles);
//
		//int vertexResolution = resolution + 1;
//
		//for (int y = 0; y < vertexResolution; y++)
		//{
			//for (int x = 0; x < vertexResolution; x++)
			//{
				//float u = (x / (float)resolution) * scale + offset.X;
				//float v = (y / (float)resolution) * scale + offset.Y;
//
				//Vector3 pointOnUnitCube = localUp
					//+ (u - 0.5f) * 2f * axisA
					//+ (v - 0.5f) * 2f * axisB;
//
				//Vector3 pointOnSphere = pointOnUnitCube.Normalized();
				//float elevation = GetFractalElevation(pointOnSphere, noise) * elevationAmplitude;
//
				//Vector3 vertex = pointOnSphere * (radius + elevation);
//
				//st.SetNormal(pointOnSphere);
				//st.AddVertex(vertex);
			//}
		//}
//
		//for (int y = 0; y < resolution; y++)
		//{
			//for (int x = 0; x < resolution; x++)
			//{
				//int i = x + y * vertexResolution;
//
				//st.AddIndex(i);
				//st.AddIndex(i + vertexResolution);
				//st.AddIndex(i + vertexResolution + 1);
//
				//st.AddIndex(i);
				//st.AddIndex(i + vertexResolution + 1);
				//st.AddIndex(i + 1);
			//}
		//}
//
		//return st.Commit();
		
		//return null;
	//}
}
