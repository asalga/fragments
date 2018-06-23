// 78 - "Rolling"
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU PI*2.

float sampleChecker(vec2 p){
  vec2 a = vec2(step(fract(p), vec2(.5)));
  return a.x == 0. && a.y == 0. ? 1. : a.x * a.y;//mix?
}

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float outside = length(max( vec2(d.x, d.y) , vec2(0., 0.) ));
  float inside = min(max(d.y,d.x),0.);
  return inside + outside;
}

float circle(vec2 p, float r){
  return length(p)-r;
}

mat2 r2d(float a){
 return mat2(cos(a),-sin(a),sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*1.;
  vec3 dirLight = vec3(0, 0, 1);
  float i;

  float rect = sdRect(p, vec2(0.8, 0.25));
  float circle = circle(p,0.25);
  // float t = sin( clamp(u_time*4., 0., 1.) );
  // float mixed = mix(rect, circle, t);
  // i += step(mixed,0.);

  i += step(rect, 0.);

  // dirLight = vec3(0., (dirLight.yz * r2d(t)));

   // x here is the primer!!!
   // use it to control perspective

  //t = u_time / (abs(p.x+u_time)*10.);
  float timeShift = p.x;
  
  // float yAngle = map(p.y,-1,1,0,PI);
  // float x = 0.;// cos(40.*PI);// ____tan____ here  
  float _y = (p.y*1.+1.)/2.*PI;
  vec3 v = vec3(0., cos(_y), sin(_y));// sin->[0..1..0]
  vec3 norm = normalize(v);

  float theta = acos(dot(norm, dirLight))/TAU;// * (1./ abs(p.y));

  float diffuse = dot(norm, dirLight);// * (1./ abs(p.y));
  i = sampleChecker( vec2(2.*PI*p.x, p.y*2.));
  //vec2(p.x*4., 1.-theta*2. )) * diffuse;

  // i *= diffuse;
  gl_FragColor = vec4(vec3(i),1.);
}
//  * r2d(t + p.x*20.);














