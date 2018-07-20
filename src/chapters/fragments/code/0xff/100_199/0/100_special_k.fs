// 100 - Special K
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;
const float TAU = (PI*2.);

float sdCircle(vec2 p, float r){
	return length(p) - r;
}

float sdRect(vec2 p, vec2 sz){
	vec2 d = abs(p) - sz;
	float _out = length(max(d, 0.));
	float _in = min(max(d.x, d.y), 0.);
	return _out + _in;
}

float checker(vec2 p, vec2 sz){
	return 0.;
}

float sdRectStroke(vec2 p){
	float _outer = step(sdRect(vec2(mod(p, .5)), vec2(0.25)), 0.);
	float _inner = step(sdRect(vec2(mod(p, .4)), vec2(0.25)), 0.);
	return _outer - _inner;
}

float sdCircleStroke(vec2 p){
	float c = step(sdCircle(p, .5), 0.);
	float innerC = step(sdCircle(p, .48), 0.);
	return c - innerC;
}

void main(){
	vec2 p = (gl_FragCoord.xy/u_res) * 2. -1.;
	float t = u_time* 0.5;
	float Sections = 6.;
	
	float r = length(p) - mod(t, 2.);
	float modAng = mod(atan(p.y, p.x), TAU/Sections);
	modAng = abs(modAng - TAU/Sections) - t;

	vec2 v = vec2( cos(modAng) *r -t,
								 sin(modAng) *r +t);
	float i;
	i = sdRectStroke(v);
	i += sdCircleStroke(vec2(v.x+cos(t*1.), v.y-sin(t*2.)));
	i += sdRectStroke(vec2(v.x+cos(t*1.)*2.,v.y-sin(t/2.)*0.2));

	gl_FragColor = vec4(vec3(i),1);
}
