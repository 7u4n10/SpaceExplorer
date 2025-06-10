shader_type spatial;

uniform float wave_speed = 1.0;
uniform float wave_strength = 0.05;
uniform float wave_scale = 2.0;
uniform vec4 ocean_color : source_color = vec4(0.0, 0.2, 0.5, 1.0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	float wave = sin(UV.x * wave_scale + TIME * wave_speed) * wave_strength;
	wave += cos(UV.y * wave_scale * 1.5 + TIME * wave_speed * 0.8) * wave_strength * 0.5;
	ALBEDO = ocean_color.rgb + vec3(wave);
	METALLIC = 0.3;
	ROUGHNESS = 0.1;
}


//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
