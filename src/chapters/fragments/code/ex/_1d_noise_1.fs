precision mediump float;

uniform vec2 u_res;
uniform float u_time;

void main(){
	vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;

	float i = fract(sin(p.x* 12342. + p.y * 3433. + u_time)*234343.);

	gl_FragColor = vec4(vec3(i), 1);
}