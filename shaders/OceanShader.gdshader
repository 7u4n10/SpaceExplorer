shader_type spatial;

render_mode blend_mix, depth_draw_opaque, cull_back, unshaded;

uniform vec4 ocean_color : source_color = vec4(0.0, 0.3, 0.6, 0.8);
uniform float wave_strength = 0.1;
uniform float time_scale = 0.5;
uniform float noise_scale = 2.0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
    float t = TIME * time_scale;
    vec3 noise_pos = NORMAL * noise_scale + vec3(t, t, t);
    float wave = sin(noise_pos.x + noise_pos.y + noise_pos.z) * wave_strength;
    
    ALBEDO = ocean_color.rgb + wave * 0.2;
    ALPHA = ocean_color.a;
}


//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
