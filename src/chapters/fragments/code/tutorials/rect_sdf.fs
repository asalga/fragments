precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  return max(d.x,d.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  i = step(sdRect(p, vec2(0.5, 0.5)), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}