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

  // float test =  // atan(p.y / p.x)/TAU;
  // 4.*((dot(normalize(p), down)+1.) /2.);
  float test = pow(abs((atan(p.y,p.x)/PI)), .8);

  float l  =      length(p);//. *pow(test, .001);

  float r = mod( 1./l + 0., 1.);
  // test = pow(test, 1.)*10.;

  
  float theta = mod( ( test * atan(p.y , p.x))/TAU *5.  , 1.);
  // theta += test*10.;

  float sample = s(vec2(r, theta));

  // float sample = atan(p.y , p.x)/;  
  // float sample = .1*abs((atan(p.y,p.x)/PI));

  gl_FragColor = vec4(vec3(sample), 1.);
}