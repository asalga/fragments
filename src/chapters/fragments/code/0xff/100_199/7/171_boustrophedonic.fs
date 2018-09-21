// 170 - "boustrophedonic"

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float Rows = 10.;
const float VertSpacing = .2;
const float PI = 3.141592658;

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  float s = sin(a);
  float c = cos(a);
  return mat2(c,s,-s,c);
}
float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float createEdge(float t, vec2 p, float cellOffset, float _){
  // multiply by 2 because we alternate even/odd between sides
  // 3   2
  // 1   0
  t -= _;
  p.y -= cellOffset;

  float edgeIdx = floor(p.y*5.)*2.;

  // extreme edges where the snake turns around.
  vec2 c = vec2(0., 0.2);
  vec2 ringSpace = mod(p, c)-(c*0.5);
  ringSpace.x -= c.y/2.;

  float edge;
  edge = step(sdRing(ringSpace, .1, .03), 0.);
  // edge = step(sdCircle(ringSpace, 0.1), 0.);

  // remove the right side of the circle/ring
  edge -= step(sdRect(ringSpace - vec2(0.1, 0.), vec2(0.1)), 0.);

  // spin until we hit PI
  // .9 since we have 10 blocks and we start on the 9th
  // not sure why 15, shouldn't it be 20?
  float _t = ((t-.9)-edgeIdx) * 15.;
  float spin = -clamp(_t, 0., PI);
  // ringSpace *= r2d((_t-edgeIdx)*PI*1.);
  ringSpace *= r2d(spin);

  // rectangle that reveals the circle
  float revealingRect = step(sdRect(ringSpace + vec2(0.1, 0.), vec2(0.1)), 0.);
  edge -= revealingRect;

  // we messed around with edge a lot, so we'll need to clamp it
  edge = clamp(edge, 0., 1.);

  return edge;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float t = u_time;// t = mod(t, 8.);
  float i;

  p.y -= 0.5;
  p.y += t/10.;

  float rowID = floor(p.y * Rows);



  float y = floor(p.y * Rows);



  float _x;
  float _y = step(y, t-1.);

  // make the snake thinner
  float localY = fract(p.y*10.);
  float horizStrip = step(localY, 1.-VertSpacing) * step(VertSpacing, localY);

  // if floored time is on the current x/horizontal line that we are on
  float rowSelected = floor(t);
  if( rowID == rowSelected){
    float ft = fract(t);
    float x = p.x;

    // if even, we go backwards, so flip x
    if(mod(rowID,2.) == 0.){
      x = 1.-x;
    }
    _x = step(x, ft);
  }

  vec2 p2 = (gl_FragCoord.xy/u_res)*2.-1.;
  float straightLineSpace = step(sdRect(p2, vec2(0.8, 1.)), 0.);

  // assemble the horizontal component of the snake
  i = _y * horizStrip * straightLineSpace;
  i += _x * horizStrip * straightLineSpace;

  i += createEdge(t,p, 0., 0.);
  i += createEdge(t,vec2(1.-p.x, p.y), .1, 1.);

  // vec2 debugGrid = mod(p, .1) * Rows;
  // vec2 debugRingGrid = mod(p, .2) * Rows;
  // gl_FragColor = vec4(vec3(i,debugRingGrid),1.);
  // gl_FragColor = vec4(vec3(edgeIdx,debugRingGrid),1.);
  gl_FragColor = vec4(vec3(i),1.);
}
