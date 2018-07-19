// 106 - 
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
	return sin(length(p)*20.) - r;
}

float sdRect(vec2 p, vec2 size){
  vec2 d = abs( p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}

void main(){
	float i;
	float t = u_time*2.;
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

	vec2 m = step(mod(p, vec2(0.1)), vec2(0.1));

	vec2 tr = vec2(sin(t), 0.);
	float c0 = step(sdCircle(p + tr, .1), 0.);
	float c1 = step(sdCircle(p - tr, .1), 0.);
	i = c0 + c1;

	float r = c0+c1;
	if(r == 2. && fract(t) < 0.5 ){
		i = 0.;
	}

	// i = step(0., sdRect(m, vec2(.5)));

	gl_FragColor = vec4(vec3(i),1);
}
