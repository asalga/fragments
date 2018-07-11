// 99 - melt
precision mediump float;

uniform vec2 u_res;
const float NumSections = 10.;

float noise(vec2 p){
	return fract(sin(p.x * 7384. + p.y * 99331.)* 303412.);
}

void main(){
	vec2 p = (gl_FragCoord.xy/u_res);
	float w = 1./NumSections;

	float i = noise(p);
	
	gl_FragColor = vec4(vec3(i),1);
}