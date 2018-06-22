precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU (PI*2.)

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time * .0;
  float mv = p.x * 1. + t;

  float corner = p.x/10.;
  float y = p.y + sin(corner * TAU + t)*0.5;
  //-.5 + sin(t)*0.15;

  i = step(fract(mv), 0.5) * step(y, -0.25);
  	  // step(fract(mv), 0.5) * step(0.,y-0.25);

  gl_FragColor = vec4(vec3(i),1.);
}