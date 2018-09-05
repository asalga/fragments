// 157 - "Over"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
// const float NumTiles = 11.0;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float diamondDist(vec2 p){
  return abs(p.x) + abs(p.y);
}

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}


void main(){
  vec2 p = (gl_FragCoord.xy/u_res)*2.-1.;
  float t = u_time*1.5;

  float NumTiles = 4.0;// + (sin(t)+1.)/2. * 20.;

  vec2 pnt = p*NumTiles;
  float halfTileCnt = (NumTiles-1.)/2.;
  vec2 fp = floor(pnt);// [0..11]

  // float dist1 = 1.-(length(fp-vec2(halfTileCnt)))/halfTileCnt;
  // float dist2 = 1.-(diamondDist(fp-vec2(halfTileCnt)))/halfTileCnt;

  // float sz1 = 0.5 * (mod(dist1 + t,     3.0) - 1.5);
  // float sz2 = 0.5 * (mod(dist2 + t-1.0, 3.0) - 1.5);

  vec2 lp = fract(pnt) - 0.5;

  // float white = step(sdCircle(lp, fract( (t + p.x*4.)/10.)  ), 0.);

  float sz = fract(t*.5 + length(p));

  float white = step(sdRect(lp, vec2(sz) ), 0.);

  // float black = step(sdRect(lp, vec2(sz2)), 0.);

  float i = white;// - black;
  // float i = black;
  gl_FragColor = vec4(vec3(i),1);
}
