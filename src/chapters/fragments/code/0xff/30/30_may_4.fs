precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
float arc(vec2 p, float w, vec2 ra, float len, float theta){
  return (1.-step(sin(PI*mod(theta+atan(p.y,p.x),PI/3.)),w))
 * step(len,ra.y) * step(ra.x,len);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a*( gl_FragCoord.xy/u_res*2.-1.);
	float i = step(length(p)-0.35,0.);
	float len = length(p);
	i += arc(p, 0.1,  vec2(0.85, 0.9), len, PI/2.);
	i += arc(p, 0.7,  vec2(0.8, 0.85), len, PI/2.);
	i += arc(p, 0.95, vec2(0., 0.7), len, 0.);
	gl_FragColor = vec4(vec3(i), 1.);
}