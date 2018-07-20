// 68 - "Reflect"
precision mediump float;

#define PI 3.141592658
#define TAU PI*2.
uniform vec2 u_res;
uniform float u_time;

mat2 r2d(float a){
  return mat2(cos(a), sin(a),-sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float time = u_time * 1.;

  vec2 p2 = p;
  p.x +=  .5;
  p2.x -= .5;

  float pLen1 = length(p);
  float s1 = pow(1./pLen1, .8);

  float pLen2 = length(p2);
  float s2 = pow(1./pLen2, .8);

  i = sin((1. + time + s1) * TAU) +
      sin((0. + time + s2) * TAU);

  float fog = 1.;
  i *= pow(pLen1, fog) * pow(pLen2, fog);
  gl_FragColor = vec4(vec3(i),1.);
}