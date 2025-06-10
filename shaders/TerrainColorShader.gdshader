shader_type spatial;

uniform float max_height = 25.0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
    // Normalize the height from 0.0 to 1.0 using the Y position
    float height_ratio = clamp(VERTEX.y / max_height, 0.0, 1.0);

    // Blend between green (low) and white (high)
	vec3 low = vec3(0.2, 0.6, 0.2);  // Grass
	vec3 mid = vec3(0.5, 0.4, 0.2);  // Rock/Dirt
	vec3 high = vec3(1.0, 1.0, 1.0); // Snow

    vec3 color = mix(low, mid, smoothstep(0.2, 0.5, height_ratio));
	color = mix(color, high, smoothstep(0.7, 1.0, height_ratio));
	ALBEDO = color;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
