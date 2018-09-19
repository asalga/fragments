precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

/*
  1) show grid
*/

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float t = u_time;

  // i = step(sdRect(p, vec2(0.25)), 0.);

  // p.y -= .5;
  // p.y += t/10.;

  float y = floor(p.y*10.);

  float md = mod(t,1.);
  i = step(p.x, md);

  // i = step(t, y);

  vec2 grid = mod(p, .1)*10.;
  gl_FragColor = vec4(vec3(i,grid),1.);
}