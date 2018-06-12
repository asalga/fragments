precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define DEG_TO_RAD(x) (PI*(x/180.))

#define EPSILON 0.001
#define MAX_STEPS 16
#define MAX_DIST 100.
#define MIN_DIST 0.0

float sdSphere(vec3 p, float r){
  return length(p) - r;
}

float sdScene(vec3 p){
  return sdSphere(p, 1.);
}

float rayMarch(vec3 eye, vec3 ray){
  float depth = 0.;

  for(int i = 0; i < MAX_STEPS; ++i){    
    vec3 scaledRay = eye + (ray * depth);
    float dist = sdScene(scaledRay);

    // close enough to surface
    if(dist < EPSILON){return depth;}

    depth += dist;

    // too far
    if(depth > MAX_DIST){return -1.;}
  }

  return MAX_DIST;
}

vec3 createRay(float fov, vec2 dim, vec2 p) {
  vec2 xy = p - (dim / 2.0);
  float z = p.y / tan(fov/ 2.0);
  return normalize(vec3(xy, -z));
}


void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i = 1.;

  vec3 ray = createRay(DEG_TO_RAD(45.), vec2(400.), gl_FragCoord.xy);
  vec3 eye = vec3(0., 0., 10.);

  float dist = rayMarch(eye, ray); 

  // no hit
  if(dist == -1.){i = 0.;}
  
  gl_FragColor = vec4(vec3(i),1.);
}