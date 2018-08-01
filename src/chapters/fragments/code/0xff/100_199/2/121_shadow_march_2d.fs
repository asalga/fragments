// 121 - "2D Shadow March"
precision highp float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;

const int MaxShadowStep = 100;
const float Epsilon = 0.001;

float sdRect(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float sdCircle(vec2 p, float r){
	return length(p) - r;
}

float sdScene(vec2 p){
	float res = 1.;
	float t = u_time * .3;

	float rad = .081;
	
	for(float it = 0.; it < 4.; ++it){

		float x = mod(it/2.+t, 2. + rad*2.) - 1. - rad;

		float cTop = sdCircle(p + vec2(x, .5), rad);	
		float cBot = sdCircle(p + vec2(x, -.5), rad);	

		res = min(res, cBot);
		res = min(res, cTop);
	}

	return res;
}

float sdSceneRender(vec2 p){
	return step(sdScene(p), 0.);
}

float shadowMarch(vec2 p, vec2 l){
	vec2 rd = normalize(l-p);
	vec2 ro = p;

	float s = 0.;
	for(int it = 0; it < MaxShadowStep; it++){
		vec2 v = ro + (rd*s);
		float d = sdScene(v);		

		// if we went father than the distance form point to light
		if( length(rd*s) > length(l-p) ){
			return 1.;
		}

		if(d < Epsilon){
			return 0.;
		}
		s += d;
	}

	return 1.;
}

void main(){
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

	vec2 lightPos = vec2(sin(u_time*2.)*0.5, sin(u_time*3.)*.3);

	float i = 1.-sdSceneRender(p);
	i -= 0.4;

	// draw light
	i += step(sdCircle(p-lightPos, 0.01), 0.);

	float visibleToLight = shadowMarch(p, lightPos);
	i -= step(visibleToLight, 0.)*.4;

	gl_FragColor = vec4(vec3(i),1);
}