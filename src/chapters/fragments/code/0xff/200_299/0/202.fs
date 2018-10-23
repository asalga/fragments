precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

void rot(inout vec2 p, in float a){
  float c = cos(a);
  float s = sin(a);

  mat2 rot = mat2(c,-s,s,c);
  p*=rot;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time;
  // p.x += t*1.2;

  i = sin(p.x*10.) * cos(p.y*10.+t);

  // rot(p, t);
  i *= sin(p.x*40.) + cos(p.y*40.);

  i += sin(length(p)*15. + t*4.)*2.;

  if(i > 0.){i=1.;}

  if(p.y > 0.){
    i = 1.-i;
  }

  // i *= cos( sin(1.*length(p)*15.));
  // i += sin(length(p)*10. + t*4.);



  gl_FragColor = vec4(vec3(i),1.);
}