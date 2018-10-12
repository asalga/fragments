
precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define PI 3.14159
#define TAU PI*2.

float sdCircle(in vec2 p, in float r){
  return length(p)-r;
}

void rot2D(inout vec2 p, in float a){
  float c = cos(a);
  float s= sin(a);

  p *= mat2(c,-s,s,c);
}

// void main(){
//   vec2 p = (gl_FragCoord.xy/u_res*2.)-1.;

//   float r = ((atan(p.y,p.x)/PI)+1.)/2.;

//   r = mod(r, TAU/4.);
//   // i = mod(i, PI/4.);

//   float i = step(sdCircle( vec2(length(r)*1., .1), 0.25), 0.);

//   gl_FragColor = vec4(vec3(i), 1);
// }


void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time;


  rot2D(p,-t/2.);
  float i = ((atan(p.y,p.x)/PI)+1.)/2.;
  i = mod(i, 0.1) * 5.;

  i = sin(i*PI+t);

  i *= sin((length(p)-t) * 20.);



  gl_FragColor = vec4(vec3(i),1.);
}





// void main(){
//   // vec2 ar = vec2(1., u_res.y/u_res.x);
//   vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
//   float density = 2.5;
//   float a = ((atan(p.y,-p.x)/PI)+1.)/2.;
//   float r = length(p)*density + a;
//   float i = step(sdCircle(p+vec2(r), 0.5), 0.);

//   // step(mod(r, 1.), 0.5);
//   gl_FragColor = vec4(vec3(i), 1.);
// }