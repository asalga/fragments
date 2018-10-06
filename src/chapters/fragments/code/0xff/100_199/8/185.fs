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

void main(){
  vec2 p = ((gl_FragCoord.xy/u_res) * 2. -1.);
  float t = u_time * .5;
  float i;

  // p += t;
  vec2 fp = floor(p*2.);

  vec2 c = vec2(0.5);
  p = mod(p,c)-c*0.5;


  float r = valueNoise(fp);
  t += r;
  // i += r;


  // main rotation
  float mainRot = PI/2. * floor(mod(t*2., 4.));
  rot(p, -mainRot);

  float sc = 1.;
  float minorRot = mod(t * PI, PI/2.) - (PI/2.);
  p += vec2(.0, sc)*c/2.;
  rot(p, minorRot);
  p -= vec2(sc/2.)*c/2.;

  i += step(sdRect(p, vec2(sc/4.)*c), 0.);
  i += grid();

  gl_FragColor = vec4(vec3(i), 1.);
}