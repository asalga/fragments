precision mediump float;uniform vec2 u_res;
#define PI 3.1415926
float cSDF(vec2 p, float r){return length(p)-r;}
void main(){
 vec2 a = vec2(1., u_res.y/u_res.x);
 vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
 float theta = atan(p.y,p.x)/PI;
  float r = length(p)/50.;
  float s = r + theta/10.;
  float i = step(0.1,mod(s,1.));
  gl_FragColor = vec4(vec3(i),1.);
}