// 135 - "Impossibilities"



precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float t = u_time*2.;

  float boxSz = 0.06;
  float floorHeight = 0.1;

  float boxJumpPos = abs(sin(t))* 0.15;
  vec2 boxPos = vec2(0.5, boxSz + floorHeight + 0.01 + boxJumpPos);
  float box = step(sdRect(p-boxPos, vec2(boxSz)), 0.);
  float floor = step(sdRect(p, vec2(1., floorHeight)),0.);

  i += floor + box;

  gl_FragColor = vec4(vec3(i),1);
}