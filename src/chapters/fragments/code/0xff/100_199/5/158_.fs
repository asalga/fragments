// 158 "Building"
precision mediump float;

#define SEVEN_EIGHTS (7./8.)
uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float bricks(vec2 p, float sc){
  float i;
  float ROWS = u_res.y/16.;
  float NUM_BRICK_LINES = 4.;
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

float valueNoise(vec2 p){
  #define Y_SCALE 45343.
  #define X_SCALE 37738.
  float x = p.x * X_SCALE;
  float y = p.y * Y_SCALE;
  return fract( sin(x+y) * 23454.);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  vec2 op = gl_FragCoord.xy/u_res;
  float i;

  float t = u_time * 0.25;
  float gt = t;

  float ft = floor(gt * 0.5);
  t = fract(gt);

  vec2 sz = vec2(0.2);
  vec2 rp = mod(p, sz)-(sz*.5);

  vec2 normed = op;
  vec2 cell = floor(normed*10.)/10.;
  cell.x += ft;

  float n = valueNoise(vec2(cell.x, cell.y))/10.;

  // cell y = [0,1,2,3....]
  // noise value = 0..1 for all values

  // vary time for each column?
  float variation = 2.;
  t *= 1. + valueNoise(vec2(cell.x, cell.x)) * variation;

  i = 1.-bricks(op/4., 1.);

  if(t > cell.y){
    float fillTime = t - cell.y;
    vec2 fillSz = sz * fillTime * 20.;

    i -= step(sdRect(rp, fillSz), 0.);    // cut a hole
    i = clamp(i, 0., 1.);
    i += step(sdRect(rp, fillSz), 0.) * (bricks(op/4., 1.));
  }

  if( mod(gt, 2.) < 1.){// flip colors
    i = 1.-i;
  }

  gl_FragColor = vec4(vec3(i),1.);
}