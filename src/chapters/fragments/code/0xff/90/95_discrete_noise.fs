// 95 - "Discrete Noise"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;  
  return fract( sin(x+y) * 23454.);
}

float smoothValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = smoothstep(0.,1.,lv);
  vec2 id = floor(p);
  float bl = valueNoise(vec2(id));
  float br = valueNoise(vec2(id)+vec2(1,0));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(vec2(id)+vec2(0,1));
  float tr = valueNoise(vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}


float sdCircle(vec2 p, float r){
	return length(p)-r;
}

float sdRect(vec2 p, vec2 sz){
	vec2 d = abs(p) - sz;
	float _out = length(max(d, 0.));
	float _in = min(max(d.x,d.y), 0.);
	return _in+_out;
}

void main(){
	vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;

	vec2 c = vec2(10.);//count of divisions
	vec2 ic = 1./c;
	vec2 np = mod(p,1./c)-.5*ic;
	float t = u_time*4.;

	// c*2 since c is divisions per side and we need total divisions
	vec2 cell = vec2( vec2(t, 0) + floor((p+1.)/2. * (c*2.)))*1.;

	// cell.x = floor(pow( (cell.x*cell.y)*u_time, 0.4));
	// cell.x += u_time + cell.y;
	// cell.x = floor(cell.x);
	cell.x *= 1.5;
	// cell.y *= (u_time)*1.1;
	
	float n = pow(smoothValueNoise(cell), 1.28);

	// 1/4 of ic since 1/2 would be a full rect
	float i = step(sdRect(np,vec2(ic*.7*n)),0.) * n;
	gl_FragColor = vec4(vec3(i),1);
}
