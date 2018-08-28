// concept from: https://www.shadertoy.com/view/4sSGD1
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
uniform sampler2D u_t0;

void main(){
	vec2 p = gl_FragCoord.xy - (u_res.xy * .5);
	vec3 col;

	for ( int i=0; i < 10; i+=1 )
	{
		float depth = 2.+float(i);
		vec2 uv = p + floor(vec2(depth));

		uv /= u_res.y;
		uv *= depth/4.0;
		uv = fract(uv/2. + 0.5);

		col = texture2D( u_t0, uv ).rgb;

		//
		if ( 1.0 - col.r < (float(i)+1.) /14.0 ) {
			break;
		}
	}

	// add contrast
	col.r = mod(col.r * 10., .85);

	// monochromize
	col = vec3(col.r);
	gl_FragColor = vec4(col,1.0);
}

// precision mediump float;

// uniform vec2 u_res;
// uniform float u_time;
// uniform sampler2D u_t0;

// void main(){
// 	vec2 pixel = gl_FragCoord.xy - u_res.xy*.5;

// 	pixel.y = 1.- pixel.y;

// 	// pixellate
// 	const float pixelSize = 2.0;
// 	pixel = floor(pixel/pixelSize);

// 	float t1 = u_time * 30000.0;
// 	vec2 offset = vec2(t1, 1600.0);
// 	offset /= pixelSize;


// 	vec3 col;
// 	for ( int i=0; i < 9; ++i )
// 	{
// 		// parallax position, whole pixels for retro feel
// 		float depth = 20.0 + float(i);
// 		vec2 uv = pixel + floor(offset/depth);

// 		uv /= u_res.y;
// 		uv *= depth/10.0;
// 		uv *= 0.5 * pixelSize;

// 		uv = fract(uv);
// 		col = texture2D( u_t0, uv ).rgb;

// 		// col = texture2D( u_t0, uv+.5 ).rgb;

// 		if ( 1.0-col.y < float(i+1)/8.0 )
// 		{
// 			col = mix( vec3(.4,.6,.7), col, exp2(-float(i)*.1) );
// 			// break;
// 		}
// 	}

// 	gl_FragColor = vec4(col,1.0);
// }

// //
// precision mediump float;

// uniform vec2 u_res;
// uniform float u_time;
// uniform sampler2D u_t0;

// const float pxSize = 1.0;

// void main(){
// 	vec2 pixel = gl_FragCoord.xy - (u_res.xy*.5);

// 	pixel = floor(pixel/pxSize);

// 	float t1 = u_time * 300.0;
// 	vec2 offset = vec2(t1, 1600.0);
// 	offset /= pxSize;

// 	vec3 col;
// 	for (int i = 0; i < 3; ++i){

// 		float depth = 20.0 + float(i);
// 		vec2 uv = pixel + floor(offset/depth);

// 		uv /= u_res.y;

// 		uv *= depth/20.0;
// 		uv *= .5* pxSize;

// 		// wrap uvs
// 		uv = fract(uv);

// 		col = texture2D( u_t0, uv ).rgb;

// 		// col = texture2D( u_t0, uv+.5 ).rgb;

// 		if ( 1.0-col.y < float(i+1)/8.0 )
// 		{
// 			// col = mix( vec3(.4,.6,.7), col, exp2(-float(i)*.1) );
// 			break;
// 		}
// 	}

// 	gl_FragColor = vec4(col,1.0);
// }