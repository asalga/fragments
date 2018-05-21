// 47 brick tower
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU PI *2.

//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}

float bricks(vec2 p, vec2 sz, float morterSz){
  float t = u_time*10.;
  vec3 forward = vec3(0., 0., 1.);
  forward.x = sin(t);
  forward.z = -cos(t);
  
  // Get the z value on the cylinder
  float z = sqrt(1.-(p.x*p.x)-(0.));
  
  // Now we can create the normal
  vec3 n = vec3(p.x, 0.,  z);
  n = normalize(n);
  float theta = acos(dot(n,forward));
  
  // Calc vector along the XZ plane
  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);

  float anY = atan(xzVec.x,xzVec.z);
  
  // p.x += anY;
  float horizSlices = .4;
  // p.x *= 
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  // p.x += step(1./4., mod(anY, .5));// * step(length(p), 1.);
// p.x = anY * theta;
  float xOffset = sz.x * step(mod(p.y, sz.y*2.), sz.y) * .5;
  float x = step(mod(p.x + xOffset, sz.x), sz.x-morterSz);
  float y = step(mod(p.y, sz.y), sz.y-morterSz);
  return x*y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  float H = 0.15;
  float W = H*2.1;
  i = step(rectSDF(p, vec2(1., a.y*2.)), 0.);
  i *= bricks(p,vec2(W,H), 0.01 );

  gl_FragColor = vec4(vec3(i),1.);
}