// 64 - "Samsara"
// based off: 
// https://www.shadertoy.com/view/MssyR7
// https://www.shadertoy.com/view/MttGz7
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define MAX_MARCH_STEPS 100
#define MIN_DIST .08
#define MAX_DIST 10.
#define EPSILON .00001

float sdSphere(vec3 p) {
  return (length(p)/10.) - 1.;
}

/**
 * Signed distance function for a cube centered at the origin
 * with width = height = length = 2.0
 */
float cubeSDF(vec3 p) {
    // If d.x < 0, then -1 < p.x < 1, and same logic applies to p.y, p.z
    // So if all components of d are negative, then p is inside the unit cube
    vec3 d = abs(p) - vec3(1.0, 1.0, 1.0);
    
    // Assuming p is inside the cube, how far is it from the surface?
    // Result will be negative or zero.
    float insideDistance = min(max(d.x, max(d.y, d.z)), 0.0);
    
    // Assuming p is outside the cube, how far is it from the surface?
    // Result will be positive or zero.
    float outsideDistance = length(max(d, 0.0));
    
    return insideDistance + outsideDistance;
}


float sdSphereSpace(vec3 p) {
  vec3 np = mod(p, vec3(1.)) * 2. -1.;
  np *= .69;

  float s = (length(np)*.5) - (.5)/2.5;

  float c = cubeSDF(np * vec3(4.,4.,1.4)) / 8.; 


  // return max(s , -c);
  return max(s , -c);
}

float rayMarch(vec3 eye, vec3 ray){
  float start = MIN_DIST;
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

/**
 * Return the normalized direction to march in from the eye point for a single pixel.
 * 
 * fieldOfView: vertical field of view in degrees
 * size: resolution of the output image
 * fragCoord: the x,y coordinate of the pixel in the output image
 */
vec3 rayDirection(float fieldOfView, vec2 size, vec2 fragCoord) {
    vec2 xy = fragCoord - size / 2.0;
    float z = size.y / tan(radians(fieldOfView) / 2.0);
    return normalize(vec3(xy, -z));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*1.;
  
  vec3 ray = normalize(vec3(p.x, p.y, -1.));

  // vec3 ray = rayDirection(45.0, u_res, gl_FragCoord.xy);
   
   float forward = 14.*-t * step(mod(t+1., 2.), 1.);
   float left = 2.*t * step(mod(t+2., 4.), 2.);
//t * step(mod(t, 2.), 1.)
  vec3 eye = vec3(left+ .5, .5 , -t);
    //forward);
  float i = rayMarch(eye, ray);

  // no hit
  if (i == MAX_DIST-EPSILON){i = 0.0;}

  float fog = pow(1./ i, 2.) * 2.;
  // i *= fog * 12.;
  i = fog;// -  (forward/200.);

  // i *= fog;
  // i *= 2./pow(2.1, i);
  gl_FragColor = vec4(vec3(i),1);
}