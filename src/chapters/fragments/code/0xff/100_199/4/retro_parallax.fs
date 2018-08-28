precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_t0;

void main(){
	vec2 pixel = gl_FragCoord.xy - u_res.xy*.5;

	pixel.y = 1.- pixel.y;

	// pixellate
	const float pixelSize = 1.0;
	pixel = floor(pixel/pixelSize);

	float t1 = u_time * 3000.0;
	vec2 offset = vec2(t1, 16000.0);
	offset /= pixelSize;


	vec3 col;
	for ( int i=0; i < 9; ++i )
	{
		// parallax position, whole pixels for retro feel
		float depth = 10.0 + float(i);
		vec2 uv = pixel + floor(offset/depth);

		uv /= u_res.y;
		uv *= depth/10.0;
		uv *= 0.5 * pixelSize;

		uv = fract(uv);
		col = texture2D( u_t0, uv ).rgb;

		// col = texture2D( u_t0, uv+.5 ).rgb;

		if ( 1.0-col.y < float(i+1)/8.0 )
		{
			// col = mix( vec3(.4,.6,.7), col, exp2(-float(i)*.1) );
			break;
		}
	}

	gl_FragColor = vec4(col,1.0);
}