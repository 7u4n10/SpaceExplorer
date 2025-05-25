using Godot;

public class TerrainNoiseProfile
{
	public int Octaves = 4;
	public float BaseFrequency = 1f;
	public float BaseAmplitude = 1f;
	public float Persistence = 0.5f;
	public float Lacunarity = 2.0f;
	public int Seed = 1337;
	public float ElevationScale = 1f;
	public FastNoiseLite.NoiseTypeEnum NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;

	private FastNoiseLite noise;

	public void Initialize()
	{
		noise = CreateNoise();
	}

	public FastNoiseLite CreateNoise()
	{
		var noise = new FastNoiseLite();
		noise.Seed = Seed;
		noise.Frequency = BaseFrequency;
		noise.NoiseType = NoiseType;
		return noise;
	}

	public float GetFractalElevation(Vector3 point)
	{
		var noise = CreateNoise();
		float elevation = 0f;
		float frequency = BaseFrequency;
		float amplitude = BaseAmplitude;

		for (int i = 0; i < Octaves; i++)
		{
			elevation += noise.GetNoise3D(point.X * frequency, point.Y * frequency, point.Z * frequency) * amplitude;
			amplitude *= Persistence;
			frequency *= Lacunarity;
		}

		return elevation * ElevationScale;
	}
}
