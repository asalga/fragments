// 112 - "Tunnel"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define Y_SCALE 45343.
#define X_SCALE 37738.
#define PI 3.141592658


float valueNoise(float seed, vec2 p){  
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * (23454. + seed));
}

float sValueNoise(vec2 p){
  vec2 lv = fract(p);
  lv = lv*lv*(3.-2.*lv);
  vec2 id = floor(p);
  float bl = valueNoise(0., vec2(id));
  float br = valueNoise(0., vec2(id)+vec2(1,0));
  // float tr = valueNoise(vec2(id)+vec2(0.,1.));
  float b = mix(bl,br,lv.x);

  float tl = valueNoise(0., vec2(id)+vec2(0,1));
  float tr = valueNoise(0., vec2(id)+vec2(1,1));
  float t = mix(tl,tr,lv.x);

  return mix(b,t,lv.y);
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max( d , 0. ));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float strokeRect(vec2 p, vec2 dim){
return step(sdRect(p, vec2(dim.x) ), 0.) - 
       step(sdRect(p, vec2(dim.x-dim.y) ), 0.);
}

mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a), cos(a));
}

vec2 getNoise(float t){
  return vec2(sValueNoise(vec2(t))*2. -1.,
              sValueNoise(vec2(t+100.))*2. -1.);
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time * .8;
  const float CNT = 30.;
  float i = 0.;
  for(float it = 0.; it < CNT; ++it){
    float chan = sValueNoise(vec2(t)) * 0.02;
    vec2 sz = vec2(it/CNT, 0.01 + chan);
    sz.x = mod(sz.x + t, 1.0);
    sz.y *= 2.;
    i += strokeRect( ( (p/2.)/pow(sz.x,2.) ) + getNoise(t*1. - sz.x)*0.85 , sz) * (1.-sz.x);
  }

  gl_FragColor = vec4(vec3(i),1);
}

