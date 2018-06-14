precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define DEG_TO_RAD(x) (PI*(x/180.))

#define EPSILON 0.001
#define MAX_STEPS 255
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

// 300/tan( ((45/2)/180) * pi)
vec3 createViewRay(float fov, vec2 sz, vec2 coord) {
  // We want to specify FOV naturlaly as the angle 
  // created from the eye to the sides of the cvs.
  // But to work with the ratios, we need to divide by
  // 2 to get a right triangle.
  float halfFov = fov/2.;

  // coord (0,0) .. (300,300)
  // (-150,-150) .. 
  vec2 xy = coord - (sz/2.);

  // the greater then dimension of y, the more
  // is assigned to the z ()
  // Why divide the result of tan(45/2) 
  float z = sz.y / tan(halfFov);

  // far left
  // -150, 0, -724  => (.2, 0. -.97)

  // return vec3(0., 0., -1.);
  // sz.y / .414;  // tan(halfFov);
  return normalize(vec3(xy, -z));

}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i = 1.;

  // increase FOV and sphere seems smaller
  vec3 ray = createViewRay(DEG_TO_RAD(45.), u_res, gl_FragCoord.xy);
  vec3 eye = vec3(0., 0., 5.);// why does 5 unit fill the entire canvas?

  float dist = rayMarch(eye, ray); 

  // no hit
  if(dist == -1.){i = 0.;}
  
  gl_FragColor = vec4(vec3(i),1.);
}