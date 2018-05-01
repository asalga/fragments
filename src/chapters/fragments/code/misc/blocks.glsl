#ifdef GL_ES
  precision mediump float;
#endif
#define PI 3.141592658

uniform vec2 u_res;

void main(){
	vec2 uv = gl_FragCoord.xy / u_res;
	vec3 col = vec3(sign(sin(uv.x * 5.0 * PI)));
	col *= step(0.1, uv.y) * step(0.2, uv.y);
	col *= 1.0 - (step(0.7, uv.y) * step(0.8, uv.y));
	gl_FragColor = vec4(col, 1.);
}