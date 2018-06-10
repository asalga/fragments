// 66 - "Revert"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define HALF_PI PI*.5
vec2 skew(vec2 p, float x){
  p.x += p.y*x;
  return p;
}
mat2 r2d(float a){
  return mat2(cos(a),sin(a),-sin(a),cos(a));
}
// From shadertoy.com/view/4sSSzG
float t(vec2 p, vec2 p0, vec2 p1, vec2 p2, float smoothness){
  vec3 e0, e1, e2;
  e0.xy = normalize(p1 - p0).yx * vec2(+1.0, -1.0);
  e1.xy = normalize(p2 - p1).yx * vec2(+1.0, -1.0);
  e2.xy = normalize(p0 - p2).yx * vec2(+1.0, -1.0);
  e0.z = dot(e0.xy, p0) - smoothness;
  e1.z = dot(e1.xy, p1) - smoothness;
  e2.z = dot(e2.xy, p2) - smoothness;
  float a = max(0.0, dot(e0.xy, p) - e0.z);
  float b = max(0.0, dot(e1.xy, p) - e1.z);
  float c = max(0.0, dot(e2.xy, p) - e2.z);
  return smoothstep(smoothness * 2.0, 1e-7, length(vec3(a, b, c)));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i, s = .5, ti = u_time*2.;
  float fractTime = fract(ti);

  float altRest = step(mod(ti, 2.), 1.);
  float rightSkew = fractTime * altRest;
  float inv = 1.-altRest;

  vec2 np = p * r2d(-ti*HALF_PI * -inv);
  // border
  vec2 b1 = step(vec2(.9), np);
  vec2 b2 = step(vec2(.9), -np);
  i += b1.x + b1.y + b2.x + b2.y;

  float t0 = 
             t(np,               vec2(0.3,1.), vec2(-0.3,1.), vec2(0.,0.7), .001) +
             t(np*r2d(PI/2.),    vec2(0.3,1.), vec2(-0.3,1.), vec2(0.,0.7), .001) +
             t(np*r2d(PI),       vec2(0.3,1.), vec2(-0.3,1.), vec2(0.,0.7), .001) +
             t(np*r2d(PI+PI/2.), vec2(0.3,1.), vec2(-0.3,1.), vec2(0.,0.7), .001);

  p *= r2d(ti*HALF_PI * -inv);
  p.y += .5;
  p = skew(p, -rightSkew);

  float tr = -rightSkew;
  float t1 = t(p, vec2(s+tr,-s+s), vec2(s+tr,s+s), vec2(-s+tr,s+s), .001);
  float t2 = t(p, vec2(-s,s+s), vec2(-s,-s+s), vec2(s,-s+s), .001);
  i += t1 + t2;
  i += t0;

  gl_FragColor = vec4(vec3(i),1.);
}