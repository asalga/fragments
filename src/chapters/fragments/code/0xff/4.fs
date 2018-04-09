precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.1415926

float elasticIn(float t){
  return sin(13.0 * t * PI/2.) * pow(2., 10. * (t - 1.));
}
float ci(vec2 p, float r){return length(p) - r;}
float capSDF(vec2 p, float w){
  vec2 a = abs(p);
  return length(vec2(max(a.x-w, 0.), a.y));
}
float hole(vec2 p){return step(0.4, 1.0 - ci(vec2(p.x/.09, p.y), 0.1));}
float eye(vec2 p, float ep){ return 1. - step(0.02, capSDF(p + vec2(-0.5, ep), 0.05));}

float body(vec2 p){
 return 1.0 - step(0.4, capSDF(p, 0.5));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res * 2. - 1.);
  
  float theta = PI/2.;
  mat2 rot = mat2(cos(theta), sin(theta), sin(theta), -cos(theta));
  float t = u_time * 10.;
  
  p *= rot;
  float hole = hole(p);
  t = u_time * 2.0;
  
  // float moveAnim = what(mod(t/5., 1.0));
  //bounceOut(mod(t/2., 1.));

  float moveAnim = 0.;

  float t1 = (t+ PI/2.) * 2.;
  moveAnim = step(0., sin(t1))  *  abs(sin(t1)) * 2.;

  float r = 4.;
  float tt = t * r;
  float faceAnim = 0.2 * step(0.0, sin(tt/(r/2.))) * sin(tt);

  float time = u_time;
  // faceAnim = bounceOut(mod(time, 1.));
  // faceAnim = circInOut(time/100.);
  float ma = moveAnim;
  p.y *= 1.0 + 4.* elasticIn(mod(moveAnim/3., 1.0));
  //ma;// p.x += 1. * ma;
  p.x += 10. * elasticIn(mod(moveAnim/3., 1.0));
 
  float eyes = eye(p, -.1 + faceAnim) + eye(p, 0.1+faceAnim);
  float nose = 1. - step(0., ci(vec2((p.x - .3), p.y/1.4 + faceAnim*1.008), 0.07));

  float i = body(p) -nose -eyes + hole;
  if(gl_FragCoord.y < 233.){discard;}
  gl_FragColor = vec4(vec3(i), 1.);
}