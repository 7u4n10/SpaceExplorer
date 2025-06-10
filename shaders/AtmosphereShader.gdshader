shader_type spatial;

render_mode unshaded, depth_draw_always, cull_back, blend_premul_alpha;

uniform vec3 light_dir = vec3(-1.0, 0.0, 1.0); // Directional light
uniform vec4 color : source_color = vec4(0.4, 0.6, 1.0, 1.0); // Atmosphere tint
uniform float intensity : hint_range(0.0, 5.0) = 1.2;
uniform float fade_start = 0.3;
uniform float fade_end = 1.0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
    float view_angle = dot(normalize(NORMAL), normalize(light_dir));
    float scatter = clamp(pow(view_angle, 1.5), fade_start, fade_end);
    scatter = (scatter - fade_start) / (fade_end - fade_start); 
    ALBEDO = color.rgb * scatter * intensity;
    ALPHA = scatter * 0.5;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
