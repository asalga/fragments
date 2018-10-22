precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

void rot(inout vec2 p, in float a){
  float c = cos(a);
  float s = sin(a);
  p *= mat2(c,-s,s,c);
}

float grid(){
  vec2 p = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(2.);
  vec2 cellSize = vec2(100.);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  return i.x + i.y;
}

void doTransform(inout vec2 p, in float sc, in float t){
  // float t = u_time  * 0.1;

  // main rotation
  float mainRot = PI/2. * floor(mod(t*2., 4.));
  rot(p, -mainRot);

  // float sc = 1.;

  float minorRot = mod(t * 1. * PI, PI/2.) - (PI/2.);
  p += vec2(.0, sc);

  rot(p, minorRot);
  p -= vec2(sc/2.);
}

void main(){
  vec2 p = ((gl_FragCoord.xy/u_res) * 2. -1.);
  float t = u_time * .4;
  float i;
  float sc = 1.;

  doTransform(p, sc,t);

  // i += step(sdRect(p, vec2(sc/2.)*1.), 0.);
  i += smoothstep(0.01, 0.001, sdRect(p, vec2(sc/2.)*1.));

  i -= smoothstep(0.01, 0.001, sdRect(p, vec2(sc/4.)*1.));
  // i -= step(sdRect(p, vec2(sc/4.)*1.), 0.);

  sc = 0.5;
  p *= 1.5;

  float mainRot = PI/2. * floor(mod(t * 2., 4.));
  rot(p, mainRot * 2.);

  doTransform(p, sc, -t*2.6);
  i += smoothstep(0.01, 0.001, sdRect(p, vec2(sc/3.)*1.));

  gl_FragColor = vec4(vec3(i), 1.);
}