// 121 - "2D Shadow March"
// shadowmarch from pixel to light
// top down? bird's eye view?
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
	float d = sdRect(p, vec2(0.25));
	float c = sdCircle(p + vec2(.5), .1);
	return min(d,c);
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
	vec2 m = vec2(u_mouse.xy/u_res)*2.-1.;
	m.y *= -1.;

	vec2 lightPos = vec2(m.x, m.y);

	float i = 1.-sdSceneRender(p);

	float visibleToLight = shadowMarch(p, lightPos);
	
	if(visibleToLight == 0.){
		i -= .5;
	}

	// i = 1.-i;

	gl_FragColor = vec4(vec3(i),1);
}