// 89 - RayMarch
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define DEG_TO_RAD(x) (PI*(x/180.))

#define EPSILON 0.001
#define MAX_STEPS 255
#define MAX_DIST 100.
#define MIN_DIST 0.

#define NORM_EPSILON 0.0001


float intersect(float a, float b){
  return max(a,b);
}

float unionSdf(float a, float b){
  return min(a,b);
}

float difference(float a, float b){
  return max(a,-b);
}


float sdSphere(vec3 p, float r){
  return length(p) - r;
}

float udBox(vec3 p, vec3 b){
  return length(max(abs(p)-b, 0.));
}

float sdScene(vec3 p){

  float box = udBox(p, vec3(0.75));
  float sphere = sdSphere(p, 1.);

  float i = intersect(box, sphere);
return i;
  return sphere;
}

// http://jamie-wong.com/2016/07/15/ray-marching-signed-distance-functions/
vec3 estimateNormal(vec3 p){
  vec3 n;

  n.x = sdScene( vec3(p.x + NORM_EPSILON, p.y, p.z) - vec3(p.x - NORM_EPSILON, p.y, p.z));
  n.y = sdScene( vec3(p.x, p.y + NORM_EPSILON, p.z) - vec3(p.x, p.y - NORM_EPSILON, p.z));
  n.z = sdScene( vec3(p.x, p.y, p.z + NORM_EPSILON) - vec3(p.x, p.y, p.z - NORM_EPSILON));

  return normalize(n);
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

mat4 viewMatrix(vec3 eye, vec3 center, vec3 up) {
  vec3 f = normalize(center - eye);
  vec3 s = normalize(cross(f, up));
  vec3 u = cross(s, f);
  return mat4(
    vec4(s, 0.0),
    vec4(u, 0.0),
    vec4(-f, 0.0),
    vec4(0.0, 0.0, 0.0, 1)
  );
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

  // vec3 eye = vec3(0., 0., 5.);// why does 5 unit fill the entire canvas?
  vec3 eye = vec3(0.0, 0.0, 5.0);

  // mat4 viewToWorld = viewMatrix(eye, vec3(0), vec3(0,1,0));
  // vec3 worldDir = (viewToWorld * vec4(ray, 0.)).xyz;


  // mat4 vm = viewMatrix(eye+vec3(1,1,0), vec3(0.), vec3(0,1,0));
  // ray = (vec4(ray,1.)*vm).xyz;

  float dist = rayMarch(eye, ray); 

  // vec3 ptest = eye + (ray * dist);
  // float dist = sdScene(scaledRay);
  // vec3 tp = eye + dist * worldDir;

  // no hit
  if(dist == -1.){
    i = 0.;
  }
  else{
    // do illumination!
    // lambert

    // vec3 dirLight = normalize(-vec3(1,0,0));
    // vec3 n = estimateNormal(ptest);

    // float intensity = dot(n,dirLight-tp);
    // i = intensity;
    // i += n.g;
    // i = 1.;
  }
  
  gl_FragColor = vec4(vec3(i),1.);
}