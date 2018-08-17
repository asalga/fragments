// 135 - "Platforms_and_Palettes"

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float timeScale = 1.25;
const float floorHeight = 0.1;
const float PI = 3.141592658;
const float TAU = PI*2.0;

// morter is at every 7/8 of a pixel across
#define SEVEN_EIGHTS (7./8.)

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdS_Rect(in vec2 p, in vec2 sz){
  return step(sdRect(p,sz),0.);
}

// pos, dim, col
float px(vec2 _, vec2 p, vec2 d, float c){
  return (c/255.) * step(sdRect(_ - p- vec2(d/2.) , d/2. ), 0.);
}

float mushroom(in vec2 p){
  float i;

  // p -= u_res/2.;
  // p -= 140.1;
  p -= 0.1;
  p *= u_res;

  // 0
  i += px(p, vec2(6, 15), vec2( 4., 1.), 160.);

  // 1
  i += px(p, vec2(5, 14), vec2( 4., 1.), 160.);
  i += px(p, vec2(9, 14), vec2( 2., 1.), 92.);

  // 2
  i += px(p, vec2(4, 13), vec2( 4., 1.), 160.);
  i += px(p, vec2(8, 13), vec2( 4., 1.), 92.);

  // 3
  i += px(p, vec2(3, 12), vec2( 5., 1.), 160.);
  i += px(p, vec2(8, 12), vec2( 5., 1.), 92.);

  // 4
  i += px(p, vec2(2, 11), vec2( 7., 1.), 160.);
  i += px(p, vec2(9, 11), vec2( 3., 1.), 92.);
  i += px(p, vec2(12, 11), vec2( 2., 1.), 160.);


  // 5
  i += px(p, vec2(1, 10), vec2( 2., 1.), 160.);
  i += px(p, vec2(3, 10), vec2( 3., 1.), 92.);
  i += px(p, vec2(6, 10), vec2( 9., 1.), 160.);

  // 6
  i += px(p, vec2(1, 9), vec2( 1., 1.), 160.);
  i += px(p, vec2(2, 9), vec2( 5., 1.), 92.);
  i += px(p, vec2(7, 9), vec2( 8., 1.), 160.);

  // 7
  i += px(p, vec2(0, 8), vec2( 2., 1.), 160.);
  i += px(p, vec2(2, 8), vec2( 5., 1.), 92.);
  i += px(p, vec2(7, 8), vec2( 5., 1.), 160.);
  i += px(p, vec2(12, 8),vec2( 2., 1.), 92.);
  i += px(p, vec2(14, 8), vec2( 2., 1.), 160.);

  // 8
  i += px(p, vec2(0, 7), vec2( 2., 1.), 160.);
  i += px(p, vec2(2, 7), vec2( 5., 1.), 92.);
  i += px(p, vec2(7, 7), vec2( 5., 1.), 160.);
  i += px(p, vec2(12, 7), vec2( 3., 1.), 93.);
  i += px(p, vec2(15, 7), vec2( 1., 1.), 160.);

  // 9
  i += px(p, vec2(0, 6), vec2( 3., 1.), 160.);
  i += px(p, vec2(3, 6), vec2( 3., 1.), 92.);
  i += px(p, vec2(6, 6), vec2( 7., 1.), 160.);
  i += px(p, vec2(13, 6), vec2( 2., 1.), 92.);
  i += px(p, vec2(15, 6), vec2( 1., 1.), 160.);

  i += px(p, vec2(0, 5), vec2( 16., 1.), 160.);

  //
  i += px(p, vec2(1, 4), vec2( 1., 1.), 160.);
  i += px(p, vec2(2, 4), vec2( 3., 1.), 92.);
  i += px(p, vec2(5, 4), vec2( 6., 1.), 255.);
  i += px(p, vec2(11, 4), vec2(3., 1.), 92.);
  i += px(p, vec2(14, 4), vec2( 1., 1.), 160.);


  i += px(p, vec2(4, 3), vec2( 8., 1.), 255.);

  // 13
  i += px(p, vec2(4, 2), vec2( 6., 1.), 255.);
  i += px(p, vec2(10, 2), vec2( 1., 1.), 160.);
  i += px(p, vec2(11, 2), vec2( 1., 1.), 255.);

  // 14
  i += px(p, vec2(4, 1), vec2( 6., 1.), 255.);
  i += px(p, vec2(10, 1), vec2( 1., 1.), 160.);
  i += px(p, vec2(11, 1), vec2( 1., 1.), 255.);


  // 15
  i += px(p, vec2(5, 0), vec2( 4., 1.), 255.);
  i += px(p, vec2(9, 0), vec2( 1., 1.), 160.);
  i += px(p, vec2(10, 0), vec2( 1., 1.), 255.);
  return i;
}

float ground(in vec2 p){
  return step(sdRect(p, vec2(1., floorHeight)),0.);
}

mat2 rot2D(in float a){
  return mat2(cos(a), sin(a), -sin(a), cos(a));
}

float boxDude(in vec2 p, in float sc, in float warp){

  // p.x -= (1.-warp)/1.;

  float t = (u_time+0.) * timeScale*PI;
  float boxSz = 0.06*sc;
  float buff = 0.01;

  float boxJumpPos = abs(sin(t)) * 0.15;
  vec2 boxPos = vec2(0.5, boxSz + floorHeight + 0. + boxJumpPos);
  // vec2 boxPos = vec2(0.5);

  vec2 tr = rot2D(t) * (p-boxPos);
  // vec2 finalTrans = p-boxPos;


  float fill = step( sdRect(tr, vec2(boxSz)) , 0.);

  return fill;
}

float obstacle(in vec2 p){
  float t = u_time*timeScale;///PI;
  float obsSz = 0.1;
  // vec2 np = p * vec2( (1.+obsSz), 1.);
  // np.x += obsSz*2.;

  float x = fract(-t);
  x *= (1. + obsSz*2.);
  x -= obsSz;

  vec2 pos = vec2(x, floorHeight+0.1);
  return sdS_Rect(p - pos, vec2(obsSz, 0.025));// * 0.5;
}

float bricks(vec2 p, float sc, float offset, float tsc){
  float t = u_time* timeScale* 0.18 * tsc;
  float i;
  float ROWS = u_res.y/16.;
  float NUM_BRICK_LINES = 4.;
  float noiseScale = 0.08;

  p.y += offset;

  p *= sc;
  p.x += t;
  // p.x += 13.;

  float n = 1.;

  // add brick lines
  // "16 tiles rows" high * lines per tile
  // morter is at every 3/4 of a pixel
  float horizLines = step(fract( p.y * ROWS * NUM_BRICK_LINES), (3./4.));
  n *= horizLines;

  // shift x every other row inside the tile
  float shift = SEVEN_EIGHTS *2.* (step(fract(p.y*ROWS*2.), SEVEN_EIGHTS/2.));
  float vertLines = step(fract( (p.x+shift/ROWS) * ROWS*2.), SEVEN_EIGHTS);
  n *= vertLines;

  return n;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float t = u_time*0.1;
  float i;

  i += sdS_Rect(p - vec2(0.6), vec2(1., 0.5)) * bricks(p, 0.25, 0., 0.5) * p.y*p.y;
  i += obstacle(p);
  i += ground(p) * bricks(p, .15, 0.300, .98) * 0.8;
  i += boxDude(p, 1., 0.);
  gl_FragColor = vec4(vec3(i),1);
}
