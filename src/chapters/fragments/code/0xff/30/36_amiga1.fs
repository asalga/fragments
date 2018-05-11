precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec2 u_tracking;
#define TAU 3.141592658*2.

void main(){
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec2 tt = u_tracking * 100.;  

  float pxGrid = clamp(tt.x, .1, 100.);
  float circleRad = 1./pxGrid;  

  float t = u_time*2.;
  vec3 ldir = vec3(cos(t), .0, sin(t));

  p -= circleRad/2.;
  vec2 BLCorner = floor(p*pxGrid)/pxGrid;
  float lenToCircle = length(p-BLCorner + circleRad);
  p = a*vec2(1.4, 1.) * (BLCorner + circleRad);

  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));  
  float theta = acos(dot(n,ldir));
  vec3 xzVec = vec3(n.x, 0., n.z);
  xzVec = normalize(xzVec);
  float anY = atan(xzVec.x/xzVec.z)/TAU + t;
  
  float div = 1.;
  float horizSlices = .7;
  float h = .25 * step(mod(p.y, horizSlices), horizSlices/2.);
  float i = step(div/4., mod(anY + h,div/2.)) *
            step(length(p), 1.);

  gl_FragColor = vec4(vec3(i), 1.);
}