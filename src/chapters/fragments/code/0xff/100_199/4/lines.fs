/*~Iridule~*/

// #extension GL_OES_standard_derivatives : enable
precision mediump float;

uniform float u_time;
uniform vec2 u_res;

//-------------------------------------------------------
#define line(d) smoothstep(.90, .98, cos(d))


void main() {
	float T = u_time;
	vec2  R = u_res;
	vec2  I = gl_FragCoord.xy;
	vec2 uv = (2. * I - R) / R.y;
	float t = 1.4 / length(uv.y);
	vec2 st = uv * t + vec2(0., t + T);
	st *= 1.;

	vec2 gv = fract(st) - .0001115;
	vec3 color = vec3(-0.);
	color *= 1. - smoothstep(.5, 1., length(gv));
	color += .61 / line(gv.y) ;
	// color += .1 / line(gv.x);
	// color *= -uv.y;

	// color *=  1.-vec3(2. * gl_FragCoord.y/u_res.y);

	if(uv.y > 0.){color = vec3(0.);}

	gl_FragColor = vec4(color, 1.);
}