// 158
// TODO:
// every 2 seconds, change y noise

precision mediump float;

uniform vec2 u_res;
uniform float u_time;

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

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float i;

  float t = u_time * 0.5;
  float ft = floor(u_time*0.5);
  t = fract(t);

  vec2 c = vec2(0.2);
  vec2 rp = mod(p, c)-(c*.5);

  vec2 normed = gl_FragCoord.xy/u_res;
  vec2 cell = floor(normed*10.)/10.;
  cell.x += ft;

  float n = valueNoise(vec2(cell.x, cell.y))/10.;

  // cell y = [0,1,2,3....]
  // noise value = 0..1 for all values

  // vary time for each column?
  float variation = 1.;
  t *= 1. + valueNoise(vec2(cell.x, cell.x)) * variation;

  if(t > cell.y + n){
    float fillTime = t - cell.y - n/2.;
    vec2 sz = vec2(0.2) * fillTime * 12.;

    i += step(sdRect(rp, sz), 0.);
  }

  if( mod(u_time, 4.) < 2.){
    i = 1.-i;
  }

  gl_FragColor = vec4(vec3(i),1.);
}