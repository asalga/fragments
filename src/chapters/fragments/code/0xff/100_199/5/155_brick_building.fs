// 155 - "Brick Building"
precision mediump float;

#define SEVEN_EIGHTS (7./8.)
const float BLOCK_SZ = 8.;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float bricks(vec2 p){
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
  vec2 p0_1 = gl_FragCoord.xy/u_res;

  float t = u_time;
  float globalTime = t * 0.5;

  vec2 sz = vec2(2./BLOCK_SZ);
  vec2 rp = mod(p, sz)-(sz*.5);

  vec2 cell = floor(p0_1*BLOCK_SZ)/BLOCK_SZ;
  cell.x += floor(globalTime);

  t = fract(globalTime);// vary time for each column?
  t *= 1. + valueNoise(cell.xx) * 2.;

  p0_1 /= 4.;//scale up so bricks are visible in a video capture
  float i = 1. - bricks(p0_1);

  if(t > cell.y){
    float fillTime = t - cell.y;
    vec2 fillSz = sz * fillTime * 8.;

    i -= step(sdRect(rp, fillSz), 0.);    // cut a hole
    i = clamp(i, 0., 1.);
    i += step(sdRect(rp, fillSz), 0.) * bricks(p0_1);
  }

  if( mod(globalTime, 2.) < 1.){// flip colors
    i = 1.-i;
  }

  gl_FragColor = vec4(vec3(i),1.);
}