precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;

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

float grid(){
  vec2 p = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(1.);
  vec2 cellSize = vec2(50.);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  return i.x + i.y;
}

void rot(inout vec2 p, in float a){
  float c = cos(a);
  float s = sin(a);
  p *=mat2(c,-s,s,c);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2. -1.;
  float t = u_time * .5;

  p.x += t;

  vec2 lp = p;
  vec2 c = vec2(0.25);
  lp = mod(p, c)-c*0.5;
  float i;
  // i = step(sdRect(p, vec2(0.125)), 0.);

  float r = valueNoise(floor(p));
  i = r;
  if(r >= .75){
    i = step(mod(p.x+t, 0.25), 0.25/2.);
  }
  else if(r > .5){
    p = fract(p);
    i = step(sin( (length(p-vec2(0.5))+t*1.) * 15.), 0.);
  }
  else if(r > .25){
   // i = step(sin( (length(lp-vec2(0.25))-t) * 10.), 0.);
   i = 1.;

    vec2 r = p;
    rot(r, PI/4.);
    // i = step(mod(r.x-t, 0.125), 0.125/2.);
     i = step(mod(r.y-t, 0.25), 0.25/2.);
  }
  else{
    // vec2 r = lp;
    // rot(r, PI/4.);
    // i = step(mod(r.x+t, 0.125), 0.125/2.);
    // i = .1;
  i = step(mod(lp.y-t*1., 0.25), 0.25/2.);
     // i = step(p.x, p.y);
     // step(mod(lp.y-t, 0.125), 0.125/2.);
  }

  // i += grid();

  gl_FragColor = vec4(vec3(i),1);
}