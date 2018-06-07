// 64 - "Samsara"
// based off: 
// https://www.shadertoy.com/view/MssyR7
// https://www.shadertoy.com/view/MttGz7
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define MAX_MARCH_STEPS 32
#define MIN_DIST 0.
#define MAX_DIST 100.
#define EPSILON .0001

float sdSphere(vec3 p) {
  return (length(p)/2.) - 1.;
}

float sdSphereSpace(vec3 p) {
  vec3 np = mod(p, vec3(1.)) * 2. -1.;
  return length(np) - .5;
}

float rayMarch(vec3 eye, vec3 ray){
  float start = 1.;
  float depth = start;

  for (int i = 0; i < MAX_MARCH_STEPS; ++i){
    
    vec3 p = eye + (ray*depth);
    
    // float d = sdSphere(p);
    float d = sdSphereSpace(p);

    // close enough to surface
    if(d < EPSILON){
      return depth;
    }
    depth += d;

    if(depth >= MAX_DIST){
      return MAX_DIST;
    }
  }
  return MAX_DIST;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  
  vec3 ray = normalize(vec3(p,1.));
  vec3 eye = vec3(0., u_time, -u_time*2.);
  float i = rayMarch(eye, ray);

  // no hit
  if (i == MAX_DIST-EPSILON){i = 0.0;}

  i = 1.5/pow(1.4,i);
  gl_FragColor = vec4(vec3(i),1);
}