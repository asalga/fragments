precision mediump float;
#define PI 3.141592658
#define TAU PI * 2.

uniform vec2 u_res;
float cSDF(vec2 p, float r){
  return length(p) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float thickness = 0.05;

  // r = a(theta)^(1/n)

  // If you spin a record and have a fly
  // walks from the center outward, it will
  // leave a trail which will form a spiral

  // float th = atan(p.y,p.x)/(3.141592658*2.8);
  // float len = length(p);
  // float t = 1.;
  // th += mod(step(len,t) * pow((t-len),1./1.5),.08);
  // // float ca = floor(th*3.)/3.;

  // float i = th;

  // i = smoothstep(0.0017, 0.00018, i);

 // i = step(0.51, mod(i, len/0.89));

  float i = step(cSDF(p, 0.5), 0.);
  
  float theta = atan(p.y/p.x);
  float r = (length(p)*5.) + (theta/TAU)/20.;

  r = step(length(p), 0.5) * r;

  float ar = step(0.1, mod(r, 0.2));
  
  if(p.x >0.){
  	ar = 1.-ar;
  }

  gl_FragColor = vec4(vec3(i,ar,i), 1.);
}