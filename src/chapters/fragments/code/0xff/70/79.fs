precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU (PI*2.)

float n(vec2 p){
  return sin(p.x*1000.+p.y);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time * 4.0;
  float mv = p.x;

  float corner = p.x/10.;
  float y = p.y;// + sin(corner * TAU + t)*0.5;

  float test = sin(floor(p.x*TAU+t));

  i = step(fract(mv*8.), 0.25) * step(y, test*1. + n((p+t/1000.))/2.);
  	  // step(fract(mv), 0.5) * step(0.,y-0.25);

  gl_FragColor = vec4(vec3(i),1.);
}