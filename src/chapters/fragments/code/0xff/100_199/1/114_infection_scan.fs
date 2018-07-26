// 114 - "Infection Scan"
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

void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float i = 0.;
  float t = 1. + u_time*1.0;
  float scale = 16.;
  float scanSpeed = t*1.;

  vec2 n = vec2(0, sValueNoise(p*scale + vec2(0, t)));
  i += strokeRect((p+vec2(t, 0  )) * n, vec2(.2, .1)) * 0.75;

  float st = 1.;
  i *= 0.8 + abs(sin(n.y+t*60.)/1.);// monitor refresh
  i += strokeRect((p+vec2(0, mod(scanSpeed, st)*st*8. - st*4.)) * n, vec2(.1, .05));

  float vignette = 1.-smoothstep(0.5, 1., abs(p.x)) *  
                   1.-smoothstep(0.5, 1., abs(p.y));
  i *= vignette;
  i = i * step(mod(gl_FragCoord.y, 4.), 2.); //scanlines

  gl_FragColor = vec4(vec3(i),1);
}

