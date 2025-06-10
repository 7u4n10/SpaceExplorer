shader_type sky;

uniform sampler2D sky_tex: source_color, filter_linear, hint_default_black;

void sky() {
	// Called for every visible pixel in the sky background, as well as all pixels
	// in the radiance cubemap.
	COLOR = texture(sky_tex, SKY_COORDS).rgb;
}
