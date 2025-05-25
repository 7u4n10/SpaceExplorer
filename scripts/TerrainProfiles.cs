using Godot;

public static class TerrainProfiles
{
	public static TerrainNoiseProfile Rocky => new TerrainNoiseProfile
	{
		Octaves = 5,
		BaseFrequency = 1.2f,
		BaseAmplitude = 1.0f,
		Persistence = 0.5f,
		Lacunarity = 2.2f,
		Seed = 1234,
		ElevationScale = 1.5f,
		NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
	};

	public static TerrainNoiseProfile Icy => new TerrainNoiseProfile
	{
		Octaves = 4,
		BaseFrequency = 0.8f,
		BaseAmplitude = 0.6f,
		Persistence = 0.4f,
		Lacunarity = 2.0f,
		Seed = 5678,
		ElevationScale = 0.8f,
		NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
	};

	public static TerrainNoiseProfile Volcanic => new TerrainNoiseProfile
	{
		Octaves = 6,
		BaseFrequency = 1.5f,
		BaseAmplitude = 1.2f,
		Persistence = 0.6f,
		Lacunarity = 2.5f,
		Seed = 9012,
		ElevationScale = 2.0f,
		NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin
	};

	public static TerrainNoiseProfile GasGiant => new TerrainNoiseProfile
	{
		Octaves = 3,
		BaseFrequency = 0.4f,
		BaseAmplitude = 0.3f,
		Persistence = 0.5f,
		Lacunarity = 1.8f,
		Seed = 3456,
		ElevationScale = 0.2f,
		NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex
	};
}
