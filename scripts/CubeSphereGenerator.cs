using Godot;
using System;
using System.Collections.Generic;

public partial class CubeSphereGenerator : Node3D
{
	[Export] public int Resolution = 32;
	[Export] public float Radius = 10f;
	[Export] public float ElevationAmplitude = 2f;
	[Export] public float NoiseFrequency = 1f;
	[Export] public int NoiseSeed = 1337;
	[Export] public int Octaves = 4;			// complexity
	[Export] public float Persistence = 0.5f;	// feature size
	[Export] public float Lacunarity = 2.0f;	// smoothness vs noise

	private FastNoiseLite noise;

	private readonly Dictionary<string, Vector3> _faceNormals = new()
	{
		{ "PlanetFace_Up", Vector3.Up },
		{ "PlanetFace_Down", Vector3.Down },
		{ "PlanetFace_Left", Vector3.Left },
		{ "PlanetFace_Right", Vector3.Right },
		{ "PlanetFace_Forward", Vector3.Forward },
		{ "PlanetFace_Back", Vector3.Back }
	};

	public override void _Ready()
	{
		noise = new FastNoiseLite();
		noise.Seed = NoiseSeed;
		noise.Frequency = NoiseFrequency;
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;

		foreach (var pair in _faceNormals)
		{
			var mesh = BuildFaceMesh(pair.Value);
			GetNode<MeshInstance3D>(pair.Key).Mesh = mesh;
		}
	}

	private ArrayMesh BuildFaceMesh(Vector3 localUp)
	{
		var mesh = new ArrayMesh();
		var st = new SurfaceTool();
		st.Begin(Mesh.PrimitiveType.Triangles);

		Vector3 axisA = new Vector3(localUp.Y, localUp.Z, localUp.X);
		Vector3 axisB = localUp.Cross(axisA);

		Vector3[,] vertices = new Vector3[Resolution, Resolution];

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
				//float elevation = GetFractalNoise(pointOnUnitSphere) * ElevationAmplitude;

				Vector3 finalVertex = pointOnUnitSphere * (Radius + elevation);

				vertices[x, y] = finalVertex;
				st.AddVertex(finalVertex);
				st.SetNormal(pointOnUnitSphere);
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

		st.GenerateNormals();
		st.Index();
		mesh = st.Commit();
		return mesh;
	}
	
	private float GetFractalNoise(Vector3 point)
	{
		float total = 0f;
		float frequency = 1f;
		float amplitude = 1f;
		float maxAmplitude = 0f;

		for (int i = 0; i < Octaves; i++)
		{
			total += noise.GetNoise3D(
				point.X * frequency,
				point.Y * frequency,
				point.Z * frequency
			) * amplitude;

			maxAmplitude += amplitude;
			amplitude *= Persistence;
			frequency *= Lacunarity;
		}

		return total / maxAmplitude;
	}

}
