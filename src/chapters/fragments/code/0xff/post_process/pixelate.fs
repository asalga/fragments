precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

void main(){
	float t = u_time * PI*2.;
	t *= .5;

	float pixelSize = floor( 1.+(sin(t)+1.)*10.);
	float i;
	vec2 p = gl_FragCoord.xy;
	vec2 c = floor( (p ) / pixelSize)*pixelSize;
	i = texture2D(u_t0,c/u_res).x;
	i = 1.-step(i, 0.);
	gl_FragColor = vec4(vec3(i),1);
}