precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI
#define COS_30 .8660256249
#define DEG_TO_RAD PI/180.
#define LG_C 0.8
float cSDF(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float blade(vec2 p, float s, float sc){
  vec2 a = abs(p);
  float distToSide = a.x * COS_30;
  float u = p.y * sc * 1.;
  float tri = clamp(max(distToSide + u, -u) - s, 0., 1.);
  float c = step(cSDF(p+vec2(0., -1.1), 0.5), 0.);
  return tri+c;
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float time = u_time * 2.;
  float count = 3.;
  float sliceSize = PI/count;
  float CC = 180./count;
  float i;

  p *= r2d(time*2.);
  float theta = atan(p.y,p.x);
  
  float pers = 1./(mod(1.-time, 1.) * 
                step(mod(time+1., 2.) , 0.75 ));
  vec2 orig_p = p;

  i += smoothstep(0.01, 0.001, cSDF(p*pers, LG_C));// LARGE CIRCLE
  i += smoothstep(0.01, 0.001, cSDF(p, .25)); // SMALL CIRCLE
  i += smoothstep(0.01, 0.001, ringSDF(p, 1.6, 0.01));// RING

  float idx = floor( ((theta+PI) / PI) * count);
  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;

  vec2 v = vec2(cos(snapped), sin(snapped)) * .8;
  float triRot = (idx-1.) * -(DEG_TO_RAD*CC);
  
  p = (p-v) * r2d(triRot);
  float T = 1.-mod(time,1.) * step( mod(time, 2.), 1.);
  p -= (v*r2d(triRot) * T); // move outwards
  
  // BLADES
  i +=  step(cSDF(orig_p, LG_C), 0.) * 
        smoothstep(0.01, 0.001, ((blade(p, .4, .5))));
  gl_FragColor = vec4(vec3(i,i,i),1.);
}