precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_tracking;
#define NUM_STRIPS 3.
#define PI 3.141592658
#define TAU PI*2.
// #define RENDER_GRID

float checkerSphere(vec2 p, vec2 origP){
  float t = u_time;
  vec3 lightDir = vec3(0., 0., 1.);
  // Get the z value on the sphere
  float z = sqrt(1.-(p.x*p.x)-(p.y*p.y));
  // Now we can create the normal
  vec3 n = vec3(p, z);
  float theta = acos(dot(n,lightDir));
  float r = 1./NUM_STRIPS;
  // Calc vector along the XZ plane
  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU + t;
  float horizSlices = .4;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  return step(1./4., mod(anY + h, 1./2.)) * step(length(p), 1.);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  // Copy p since the grid needs a 'clean' version of p.
  float count = 50. * u_tracking.x;
  vec2 circP = p*count + 1.;
  // snap to closest bottom corner
  circP = circP-floor(circP/2.)*2.-1.;
  float i = checkerSphere(circP, p);
  #ifdef RENDER_GRID
    vec2 cellSize = vec2(count/2.); 
    vec2 modp = mod(p, cellSize);
    modp -= cellSize/2.;
    vec2 lineWidthInPx = vec2(0.01);
    vec2 grid = step(mod(p, cellSize), lineWidthInPx);
    i += grid.x + grid.y;
  #endif
  gl_FragColor = vec4(vec3(i), 1.);
}