precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float circleSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p += vec2(-.4, -.8);
  float i = 1.-smoothstep(.23, .24,circleSDF(p,.3));
  i += smoothstep(0.,.1,sin(u_time*2.-atan(p.y,p.x) * 10.));
  gl_FragColor = vec4(vec3(1.-i), 1.);
}