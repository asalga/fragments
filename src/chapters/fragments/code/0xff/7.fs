precision mediump float;
uniform vec2 u_res;
uniform float u_time;

#define PI 3.1415926
#define TAU 2.*PI
#define O 1.
#define Z 0.

float sphereSDF(vec2 p, float r){
  return length(p) - r;
}

float circleSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a* (gl_FragCoord.xy / u_res * 2. -1.);
  float t = u_time;

  vec3 sunDir = vec3(cos(t), Z, sin(t));
  sunDir = normalize(sunDir);

  // vec3 v = 1. - vec3(length(p) - 0.25);
  // vec3 c = vec3(step(circleSDF(p, 0.5), 0.5));
  // vec3 v = vec3(step(circleSDF(p, 0.5), 0.5));

  if(length(p) > 1.0){
    discard;
  }

  // Need to somehow create z coordinate from 2D
  // float z = (sin(p.y * 1.)  - p.y) + (cos(p.x * 1.) - p.x);
  float z = sqrt(1. - (p.x*p.x) - (p.y*p.y));

  // float z = 0.0;

  // B = [1,0,0]
  // cos(1 * PI) + sin(0 * PI) 
  // cos(PI) + sin(0)
  // -1 + 0

  vec3 v = vec3(p.x, p.y, z);
  v = normalize(v);
  
  vec3 i = vec3(dot(v, sunDir));
  // c = vec3(step(c.x, 0.5));
  // i = step(vec3(0.9), i);
  gl_FragColor = vec4(i, 1.);
}