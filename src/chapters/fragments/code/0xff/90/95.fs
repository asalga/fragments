precision mediump float;

uniform vec2 u_res;

float sdCircle(vec2 p, float r){
	return length(p)-r;
}


void main(){
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

	float i = 1.-step(0., sdCircle(p, .5));

	gl_FragColor = vec4(vec3(i),1);
}