precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define PI 3.14159

float cSDF(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  
  float theta = ((atan(p.y,-p.x)/PI)+1.)/2.;
  float r = length(p);
  float t = u_time/10.;
  float sp = r + theta;

  // r,theta
  // r =  b * theta
  

  float i = step(mod(sp, 1.), 0.5);

  vec2 path = vec2(r+t, 1.);

  float c = step(cSDF(p+path, 0.1),0.0);

  i += c;
  i=c;

  gl_FragColor = vec4(vec3(i), 1.);
}