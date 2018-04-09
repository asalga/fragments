precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.1415926

float ci(vec2 p, float r){return length(p) - r;}

float capSDF(vec2 p, float w){
  vec2 a = abs(p);
  return length(vec2(max(a.x-w, 0.), a.y));
}

float hole(vec2 p){return step(0.4, 1.0 - ci(vec2(p.x/.09, p.y), 0.1));}
float eye(vec2 p, float ep){ return 1. - step(0.02, capSDF(p + vec2(-0.5, ep), 0.05));}

float capSDFOutline(vec2 p, float w, float th){
  float test = 1.0 - step(0.5,capSDF(p, w));
  float t2 = 1.0 - step(0.5,capSDF(p/0.95, w));

  return test - t2;
  // vec2 a = abs(p);
  // return length(vec2(max(a.x-w, 0.), a.y)) - (length(vec2(max(a.x-w, 0.), a.y)) * 0.01);	
}

float body(vec2 p){
 if(p.x < 0.0){ return 0.;}
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
  t = u_time * 4.0;
  float moveAnim = step(0., sin(t)) * abs(sin(t)) * 2.4;
  float tt = (t-10.) * 4.;
  float faceAnim = 0.2 * step(0.0, sin(tt/2.) * 2.0 ) * sin(tt);
  float ma = moveAnim;
  p.y *= 1. + ma;
  p.x += 1. * ma;
 
  float eyes = eye(p, -.1 + faceAnim) + eye(p, 0.1+faceAnim);
  float nose = 1. - step(0., ci(vec2((p.x - .3), p.y/1.4 + faceAnim*1.008), 0.07));

  float i = body(p) -nose -eyes + hole;
  if(gl_FragCoord.y < 233.){discard;}
  gl_FragColor = vec4(vec3(i), 1.);
}