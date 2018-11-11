// 12 - Dark Star
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float circleSDF(vec2 p, float r){
  return length(p)-r;
}

void rot(inout vec2 p, float a){
  float c = cos(a);
  float s = sin(a);
  p *= mat2(c, s, -s, c);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  // p += vec2(-.4, -.8);
  rot(p, 3.14/4.);
  float i = 1.-smoothstep(.23, .24,circleSDF(p,.0));
  float l = length(p)*20.;
  i += smoothstep(0.,.1,sin(u_time*2.-atan(p.y,p.x) * sin(l*2.)));
  gl_FragColor = vec4(vec3(1.-i), 1.);
}