// melt - "97"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float circle(vec2 p, float r){
	return length(p) - r;
}

float valueNoise(vec2 p){
	return fract(sin(p.x * 34234. + p.y * 89713.) * 143914.);
}


void main(){
	vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;
	float i;
	float t = u_time;

	i = valueNoise(vec2(p)*2000. + pow(t, t));

	float c = circle(p, 1.3);

	i = c+ i;
	gl_FragColor = vec4(vec3(i),1);	
}