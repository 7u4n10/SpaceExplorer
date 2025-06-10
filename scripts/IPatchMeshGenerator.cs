using Godot;

public interface IPatchMeshGenerator
{
	// version 1
	//ArrayMesh GeneratePatchMesh(
		//Vector3 localUp,
		//Vector3 axisA,
		//Vector3 axisB,
		//Vector2 offset,
		//float scale,
		//float radius,
		//FastNoiseLite noise,
		//float elevationAmplitude,
		//int resolution
	//);
	
	ArrayMesh GeneratePatchMesh(
		Vector3 localUp,
		Vector3 axisA,
		Vector3 axisB,
		Vector2 offset,
		float scale,
		float radius,
		TerrainNoiseProfile profile,
		int resolution
	);
}
