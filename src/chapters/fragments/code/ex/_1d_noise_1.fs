precision mediump float;

uniform vec2 u_res;

void main(){
	vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;

	vec2 i = fract(sin(p.x* 12342. + p.y * 3433.)*234343.);

	gl_FragColor = vec4(vec3(i), 1);
}