// 99 - melt
// Add porabola influence
// Change color every turn

precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float NumSections = 510.;

float valueNoise(vec2 p){
	return fract(sin(p.x * 7384. + p.y * 99331.)* 303412.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = lv*lv*(3.-2.*lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);
  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);
  return mix(b,t,lv.y);
}

vec2 getSection(float x, float multiplier){
  return vec2(floor((x*multiplier)*NumSections)/NumSections, 0.);
}

float remap(float v, float low1, float high1, float low2, float high2){
  return low2 + (v - low1) * (high2 - low2) / (high1 - low1);
}

void main(){
	vec2 p = (gl_FragCoord.xy/u_res);
	float i;
	
	float turn;
	float porabolaInfluence;
	
	float t = u_time*1.;
	t = fract(t);

	vec2 accSection = getSection(p.x + 4., 1.);
	float a = smoothValueNoise(accSection);
	a = remap(a, 0., 1., 0.2, 0.8);
	float deltaV = a * t;

	vec2 velSection = getSection(p.x + turn, 35.);
	velSection.x = remap(velSection.x, 0., 1., 0.4, 0.5);
	float v = smoothValueNoise(velSection) + deltaV;
	
	float d = v * t;

	// TODO: change color every turn
	i = step(p.y, 1.-d);
	gl_FragColor = vec4(vec3(i),1);
}



















