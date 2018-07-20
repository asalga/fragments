precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
#define TAU 2. * PI

float rSDF(vec2 p, float r, float w){
  return abs(length(p) - r *0.5) - w;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x );
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. -1.);
  float i = 0.;
  for(float f = 0.;f < TAU; f += (TAU/6.)){
    i += step(rSDF(p + 0.42*vec2(cos(f), sin(f)), .85, .001), 0.01);
  }
  gl_FragColor = vec4(vec3(i), 1.);
}