// 106 - Moiré 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
	return sin(length(p)*50.) - r;
}

void main(){
	float i;
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
	float t = u_time*3.;

	float d = .5;
	vec2 tr = vec2(cos(t) * d, 0.);
	float c0 = step(sdCircle(p, .021), 0.);
	float c1 = step(sdCircle(p + tr, .021), 0.);
	i = c0 + c1;

	float r = c0+c1;
	if(r == 2. && tr.x < 0.){
		i = 0.;
	}

	gl_FragColor = vec4(vec3(i),1);
}
