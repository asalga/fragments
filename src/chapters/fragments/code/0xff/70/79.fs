// 79 - 
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, length(p)-r);
}

void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy / u_res * 2. -1.);
  float t = u_time * 0.91;

  // p += 0.4;

  vec3 l = normalize(vec3(.7, .7, 0.9));
  float z = sqrt(1. - p.x*p.x - p.y*p.y);
  
  vec3 v1 = normalize(vec3(p.x, p.y, z));
  vec3 v2 = normalize(vec3(p.x, p.y, -z));

  float c = smoothstep(0.1, 0.0001, sin(t*10. + dot(v1,l)*30.));
  float c2 = smoothstep(0.1, 0.0001, sin(t*10. + dot(v2,l)*30.) )/2.;

  c += c2;
  // c += circle(p-=1.5, 0.7);
  gl_FragColor = vec4(vec3(c), 1.);
}