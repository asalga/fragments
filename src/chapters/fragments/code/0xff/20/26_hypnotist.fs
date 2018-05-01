precision mediump float;
uniform vec2 u_res;
uniform vec2 u_tracking;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float th = atan(p.y/p.x)/3.141592658;
  float len = length(p);
  float t = u_tracking.x*2.;
  th += step(len,t) * pow((t-len),2.);
  float ca = floor(th*3.)/3.;
  float i = smoothstep(0.21, 0.215,th-ca);
  gl_FragColor = vec4(vec3(i), 1.);
}