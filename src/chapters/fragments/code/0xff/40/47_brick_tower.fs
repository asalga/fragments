// 47 brick tower
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658
#define TAU PI * 2.

#define BRICK_H .2
#define BRICK_W BRICK_H * 2.1

//from book of shaders
float rectSDF(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}
//from book of shaders
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,178.233)))*43758.5453123);
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
  
  float horizSlices = .4;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  // p.x += step(1./4., mod(anY, .5));// * step(length(p), 1.);
  // p.x = anY * theta;

  // give each row an index then feed that index into rand
  float idx = floor(p.y/BRICK_H);
  float randRowOffset = 0.;//random(vec2(0., idx)) * .8;

  float halfXOffset = (step(mod(p.y, sz.y*2.), sz.y)*2. -1.) * 
                        (u_time);

  float x = step(mod(p.x + halfXOffset + randRowOffset, sz.x), sz.x-morterSz);
  float y = step(mod(p.y, sz.y), sz.y-morterSz);
  return x*y;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  p.y += u_time;
  p.x += p.y*2.;

  // i = step(rectSDF(p, vec2(1., a.y*2.)), 0.);
  i = bricks(p, vec2(BRICK_W, BRICK_H), 0.12 );

  gl_FragColor = vec4(vec3(i),1.);
}