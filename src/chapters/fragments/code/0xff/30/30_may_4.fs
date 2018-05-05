precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
float a(vec2 p, float w, vec2 ra, float len, float theta){
  return (smoothstep(w,w+.04,
      sin(PI*mod(theta+atan(p.y,p.x),PI/3.))))
      * step(len,ra.y) * step(ra.x,len);
}
void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar*(gl_FragCoord.xy/u_res*2.-1.);
  float i = 1.-smoothstep(0., 0.01,length(p)-.35);
  float len = length(p);
  i += a(p, .1, vec2(.85, .9),  len, PI/2.);
  i += a(p, .7, vec2(.80, .85), len, PI/2.);
  i += a(p, .85,vec2(.00, .7),  len, 0.);
  gl_FragColor = vec4(vec3(i), 1.);
}