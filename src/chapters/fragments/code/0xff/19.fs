precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float cSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  p.x += step(p.y,0.)*.05*sin(5.*u_time+p.y*99.)*2.*p.y;
  float i = 1.-smoothstep(.0,.01, cSDF(p,.8));
  i = abs(step(0., p.y)-i);
  gl_FragColor = vec4(vec3(i), 1.);
}