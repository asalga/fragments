// Tower
// TODO:
// - warp space for bricks
// - move eye into tower
// - add stars

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658

float udRoundBox(vec2 p, vec2 b, float r){
  return length(max(abs(p)-b,0.0))-r;
}

vec2 flipX(vec2 v){
  return v * vec2(-1., 1.);
}
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
}

float random1D(float v) {
  // return fract(sin(v*12.9898),178.233);
  return fract(sin(v));
}

// from IQ
float impulse(float k, float x){
  float h = k * x;
  return h * exp(1. - h);
}
float cSDF(vec2 p, float r){
  return length(p) - r;
}
float cInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,-i), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(0.,i), r));
  return a*b;
}
float cInterStroke(vec2 p, float r, float i){
  float a = cInter(p,r,i);
  return a - cInter(p,r-.06,i);
}
//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float srectSDF(vec2 p, vec2 size){
  return step(rectSDF(p, size), 0.);
}

float bolt(vec2 p, vec2 pos){
  float i;
  p += pos;
  p.x -= p.y*1.0;
  float W = 0.1;
  float H = 0.3;
  i += srectSDF(p, vec2(W,H));
  i += srectSDF(p - vec2(W, -H), vec2(W/2.,H/2.));
  i += srectSDF(p - vec2(W*1.5, -H*1.5), vec2(W/3.,H/3.));
  return i;
}

float eye(vec2 p, vec2 pos){
  p/= 0.5;
  float i = cInter(p-vec2(0.,.6), .6, 0.3);
  i -= cInter(p-vec2(0.,.6), .55, 0.3);
  i += step(cSDF(p-vec2(0.,0.7), 0.2), 0.);
  return i;
}

float moon(vec2 p){
  p += vec2(0.6, -1.2);
  p/=0.2;
  float t = mod(0.9, 2.*PI);
  vec3 s = vec3(cos(t), .0, sin(t));
  float z = sqrt(1. - pow(p.x,2.) - pow(p.y,2.));
  vec3 v = normalize(vec3(p.x, p.y, z));
  vec3 i = vec3(smoothstep(.1,.11,dot(v, s)) + 
  				smoothstep(.48, .49, 1.-cSDF(p, .5)) -
  				smoothstep(.38, .39, 1.-cSDF(p, .37)));
  return i.x;
}
float towerShape(vec2 p, vec2 towerPos){
  float i = 0.;

  // tower body
  vec2 towerBodySz = vec2(0.4, .8);
  i += step(rectSDF(towerPos+ vec2(0., 0.7), towerBodySz), 0.);

  // clamp
  if(p.y > 0.){
    p.y = 2.;
  }

  // tower top bricks
  float topBrickSz = 0.15;
  // i += step(mod(p.x-0.15, topBrickSz), topBrickSz/2.)*
  	   // step(rectSDF(p-vec2(0., 0.14), vec2(0.5, 0.05)), 0.);

  // tower top
  vec2 towerTopSz = vec2(0.5, .12);
  // i += step(rectSDF(towerPos-vec2(0.,.1), towerTopSz), 0.);

  // TOWER SLOPE LEFT
  float slopeAmt = 12.;
  float v = impulse(slopeAmt, -p.x);
  i += step(p.y+1.4, v*2.);

  // TOWER SLOPE RIGHT
  v = impulse(slopeAmt, p.x);
  i += step(p.y+1.4, v*2.);

  return i;
}

float bricks(vec2 p, vec2 sz, float morterSz){
  float xOffset = sz.x * step(mod(p.y, sz.y*2.), sz.y) * .5;
  float x = step(mod(p.x + xOffset, sz.x), sz.x-morterSz);
  float y = step(mod(p.y, sz.y), sz.y-morterSz);
  return x*y;
}


void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec2 towerPos = p + vec2(0., 0.0);
  float i = 0.;

  // Tower shape + bricks
  float brickH = 0.15;
  float brickW = brickH*2.1;
  float brickSpacing = 0.01;
  vec2 circularShape = p;

  i += towerShape(p, towerPos) * 
      bricks(p, vec2(brickW, brickH), brickSpacing);
  i += eye(p, vec2(0.));
  i += moon(p);
  // i *= specks(p);

  // if( random1D(u_time) > 0.01){
  float r = random(vec2(u_time/1000.));

  // if( r > .98 ){
    i += bolt(p-vec2(1.4, 0.), vec2(0.5, 0.5));
    i += bolt( flipX(p)-vec2(1.4, 0.)  , vec2(0.5, 0.5));
  // }
  // i += eye((p+vec2(-.0,-1.4)) * vec2(1., -1.), vec2(0.));


  p.x *= u_time/1000000.;
  i += step(random(p), 0.0008);

  gl_FragColor = vec4(vec3(i),1.);
}