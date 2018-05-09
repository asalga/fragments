precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define PI 3.14159

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  
  float density = 1.5;
  float thickness = 0.25;

  float a =((atan(p.y,-p.x)/PI)+1.)/2.;
  float r = length(p)*density + a/4.;

  float t = u_time/420.;
  // t = 0.5;
  float len = length(p);

  float timeAround = ((1.-len)*a) - mod(len, 0.1)*a;

  // float progress = ((length(p))) + (a/10.);
  if(timeAround < 0.015){
    discard;
  }

  if(timeAround < t){
    discard;
  }

  float progress = timeAround;
  //0.4;//(length(p)/1.) * (a/3.);

  float i = mod(r, .15);

  
  float c = ((atan(p.y,-p.x)/PI)+1.)/2.;

  t = mod(t*10.,4.) ;
  c = step(c, t);
  c = 1.;

  if(progress < t){
    c = 0.;
  }


  gl_FragColor = vec4(vec3(c), 1.);
}







