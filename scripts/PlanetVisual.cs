using Godot;
using System;

public partial class PlanetVisual : Node3D
{
	[Export] public int Resolution = 32;
	[Export] public float Radius = 10f;
	[Export] public float ElevationAmplitude = 2f;
	[Export] public float NoiseFrequency = 1f;
	[Export] public int NoiseSeed = 1337;
	[Export] public FastNoiseLite.NoiseTypeEnum NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;

	private FastNoiseLite noise;

	public override void _Ready()
	{
		GenerateSurface(Radius);
	}

	public void GenerateSurface(float radius)
	{
		Radius = radius;
		noise = new FastNoiseLite
		{
			Seed = NoiseSeed,
			Frequency = NoiseFrequency,
			NoiseType = NoiseType
		};

		foreach (var child in GetChildren())
		{
			if (child is MeshInstance3D faceMesh)
			{
				GenerateFaceMesh(faceMesh, faceMesh.Name, radius);
			}
		}
	}

	private void GenerateFaceMesh(MeshInstance3D meshInstance, string faceName, float radius)
	{
		Vector3 localUp = faceName switch
		{
			"PlanetFace_Up" => Vector3.Up,
			"PlanetFace_Down" => Vector3.Down,
			"PlanetFace_Left" => Vector3.Left,
			"PlanetFace_Right" => Vector3.Right,
			"PlanetFace_Forward" => Vector3.Forward,
			"PlanetFace_Back" => Vector3.Back,
			_ => Vector3.Up
		};

		var st = new SurfaceTool();
		st.Begin(Mesh.PrimitiveType.Triangles);

		Vector3 axisA = new Vector3(localUp.Y, localUp.Z, localUp.X);
		Vector3 axisB = localUp.Cross(axisA);

		for (int y = 0; y < Resolution; y++)
		{
			for (int x = 0; x < Resolution; x++)
			{
				Vector2 percent = new Vector2(x, y) / (Resolution - 1);
				Vector3 pointOnUnitCube =
					localUp +
					(percent.X - 0.5f) * 2f * axisA +
					(percent.Y - 0.5f) * 2f * axisB;

				Vector3 pointOnUnitSphere = pointOnUnitCube.Normalized();
				float elevation = noise.GetNoise3D(
					pointOnUnitSphere.X,
					pointOnUnitSphere.Y,
					pointOnUnitSphere.Z
				) * ElevationAmplitude;

				Vector3 finalVertex = pointOnUnitSphere * (radius + elevation);

				st.SetNormal(pointOnUnitSphere);
				st.AddVertex(finalVertex);
			}
		}

		for (int y = 0; y < Resolution - 1; y++)
		{
			for (int x = 0; x < Resolution - 1; x++)
			{
				int i = x + y * Resolution;

				st.AddIndex(i);
				st.AddIndex(i + Resolution);
				st.AddIndex(i + Resolution + 1);

				st.AddIndex(i);
				st.AddIndex(i + Resolution + 1);
				st.AddIndex(i + 1);
			}
		}

		var arrayMesh = st.Commit();
		meshInstance.Mesh = arrayMesh;
	}
}
