precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define TAU 3.14159265834*2.
#define PI 3.14159265834

float s(vec2 p){
  vec2 a = vec2(step(p, vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/ u_res * 2. -1.);
  // too lazy to rotate right now, perp it.
  p = vec2(-p.y, p.x);
  float test = pow(abs((atan(p.y,p.x)/PI)), .8);

  float lenDeform = abs((atan(p.y,p.x)/PI));

vec2 newP = p;
//   if(newP.x > 0.){
//   	// p += pow(p, 2.1);
// newP.x *= 2.4;
//   }
//   else{
//   newP.x *= 9.14;	
//   }

  float l = length(newP);



  float r = mod(1./l - 0., 1.);
  float theta = mod( ( test * atan(p.y , p.x))/TAU *2., 1.);

  float sample = s(vec2(r, theta)) * pow(l, 2.);
  // float sample = atan(p.y , p.x)/;  
  // float sample = .1*abs((atan(p.y,p.x)/PI));

  gl_FragColor = vec4(vec3(sample), 1.);
}