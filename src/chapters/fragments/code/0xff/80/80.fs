precision mediump float;

uniform vec2 u_res;
uniform float u_time;

#define PI 3.141592658

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float outside = length(max( vec2(d.x, d.y) , vec2(0., 0.) ));
  float inside = min(max(d.y,d.x),0.);
  return inside + outside;
}

float circle(vec2 p, float r){
  return length(p)-r;
}

float combine(vec2 pg){
  return 0.;
}

mat2 r2d(float a){
 return mat2(cos(a),-sin(a),sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*7.;
  vec3 dirLight = vec3(0, 0, 1.);

  float rect = sdRect(p, vec2(0.8, 0.25));
  float circle = circle(p,0.25);
  // float t = sin( clamp(u_time*4., 0., 1.) );
  // float mixed = mix(rect, circle, t);
  // i += step(mixed,0.);

  i += step(rect, 0.);

   // x here is the primer!!!
   // use it to control perspective

   //t = u_time / (abs(p.x+u_time)*10.);
  float timeShift = p.x;

  // t = t*timeShift;
  // p.x += t*0.5;
  // p.x *= 3.;
  float _x = 0.;// cos(40.*PI);// ____tan____ here
  float y = (cos((p.y+1.)/2.*PI*4.));
  float z = sin((p.x+1. *.5 )/2.*PI*5.);

  vec2 _yz = vec2(y,z)  * r2d(t + p.x*20. );
  vec3 v = vec3(_x, _yz.x, _yz.y);// y = -1 .. 1
  
  vec3 norm = v;//normalize(v);

  float d = dot(norm, dirLight);

  vec3 view = vec3(0., 0., 1.);
  float sd = dot(norm, normalize(dirLight + view));
  float spec = pow( clamp(sd, 0., 1.), 22.);
  float intensity = d + spec;

  i = intensity;

  gl_FragColor = vec4(vec3(i),1.);
}