precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
#define TAU 2.*PI
#define COS_30 0.8660256249
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float triSDF(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc;
  return max(distToSide + u, -u) - s;
}
float triOutline(vec2 p){
  p *= 0.7;
  return  step(triSDF(p-vec2(0., 0.18), .25, 0.5), 0.) * 
          (1.-step(triSDF(p-vec2(0., 0.15), .20, 0.5), 0.));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float count = 3.;  
  float sliceSize = PI/count;
  float theta = atan(p.y,p.x);
  float idx = floor((theta+PI)/PI * count);
  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;
  vec2 v = vec2(cos(snapped), sin(snapped));
  float i = triOutline(p) + 
            triOutline(p*vec2(1.,-1.)) - 
            step(ringSDF(p - v, 0.4, 0.025), 0.);
  gl_FragColor = vec4(vec3(i), 1.);
}