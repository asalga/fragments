precision highp float;

uniform vec2 u_res;
uniform float u_time;

void rot(inout vec2 p, in float a){
  mat2 r = mat2(cos(a),sin(a),-sin(a),cos(a));
  p *= r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res *2.-1.;
  float t = u_time* 0.25;
  float len = (length(p)-t) * 10.;
  float i = abs(sin(len));
  if (step(fract(p.x+t*2.), 0.5)*sin(len) > 0.){
    i = 1.-i;
  }
  gl_FragColor = vec4(vec3(i),1);
}