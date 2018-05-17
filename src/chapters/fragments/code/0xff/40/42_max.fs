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
  
  float tri = max(distToSide + u, -u) - s;
  tri = clamp(tri, 0., 1.);

  float c = step( cSDF(p+vec2(0., -1.1), 0.5), 0.);
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
  float CC = 180./(count);
  p *= r2d(time*2.);
  float theta = atan(p.y,p.x);
  
  float i;
  float T = 1.-mod(time,1.) * 
            step( mod(time, 2.), 1.);

  float pers = 1./(mod(1.-time, 1.) * 
                step(mod(time+1., 2.) , 0.75 ));
  vec2 op1 = p * pers;
  vec2 op = p;

  i += step(cSDF(op1, LG_C), 0.);// LARGE CIRCLE
  i += step(cSDF(p, .25), 0.); // SMALL CIRCLE
  i += step(ringSDF(p, 1.6, 0.01), 0.);// RING

  //               0..TAU      0..1   [0,1,..c-1]
  float TH = theta;
  float idx = floor( ((TH+PI) / PI) * count);

  float snapped = -PI + (idx * sliceSize) + sliceSize/2.;

  vec2 v = vec2(cos(snapped), sin(snapped)) * .8;

  // remap 0..2 to 1..-1
  float triRot = (idx-1.) * -(DEG_TO_RAD*CC);
  
  p = (p-v) * r2d(triRot);

  // move outwards
  p -= (v*r2d(triRot) * T);   
  
  // BLADES
  i +=  step(cSDF(op, LG_C), 0.) * 
        step(((blade(p, .4, .5))), 0.);
  gl_FragColor = vec4(vec3(i,i,i),1.);
}