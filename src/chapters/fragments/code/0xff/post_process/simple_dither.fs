/*
	Simple Dithering

	We use a texture instead of a dither matrix since
	working with uniform matricies are not as flexible.
*/
precision highp float;

uniform sampler2D u_t0;
uniform sampler2D u_ditherTex;
uniform vec2 u_res;
uniform float u_time;

float ditherLookup(vec2 p){
	p.y = u_res.y - p.y;

	const float sz = 3.;
	//0,0,0,3,3,3,6,6,6
	vec2 cellIdx = floor(p/vec2(sz))*sz;

	vec2 median = cellIdx + 1.;//center
	vec2 texCoord = median/u_res;
	float medianCol = texture2D(u_t0, texCoord).x;

	// [0,0] [1,0] [2,0]
	// [0,1] [1,1] [2,1]
	// [0,2] [1,2] [2,2]
	vec2 localCoord = mod(p, sz);

	// use localCoord to sample dither matrix, stored as a texture
	localCoord /= sz; // remap to 0..1
	float ditherThreshold = texture2D(u_ditherTex, localCoord).x;
	return 1.-step(medianCol, ditherThreshold);
}

void main(){
	// don't remap to keep things easier
	vec2 c = gl_FragCoord.xy;

	// c = floor(c/.);

	// c = floor(gl_FragCoord.xy*2.)/(u_res);
	c = floor(c/2.)*2.;
	float i = ditherLookup(c);
	// float i = texture2D(u_t0, c/u_res).x;

	gl_FragColor = vec4(vec3(i),1);
}