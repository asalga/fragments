precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
  return step(sdCircle(p,r), .0);
}
float sdRing(vec2 p, float r, float w){
  return abs(length(p) - (r*.5)) - w;
}
float ring(vec2 p, float r, float w){
  return step(sdRing(p, r, w), 0.);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float time = mod(u_time*.5, 1.);
  float i;
  float t = time;
  float sw = 0.001;
  float r = 1.;
  float pos = .6;
  // float TAN_ = tan(.647);
  float theta = atan(pos/1.);

  // p.y += time/1.25;
  // r += time;
  

  float test = tan(theta) * t;
  vec2 v = vec2(0., -pos + test );
  vec2 v1 = vec2(0., pos + test );
  
  vec2 p1 = p;
  vec2 p2 = p;

  p1 += v;
  p1 /= 1. + t*9.;

  p2 -= v1;
  p2 /= 1. + t*9.;

  
  i += ring(p, .8*r, sw);
  i += ring(p1, .2*r, sw);
  // i += ring(p2, .2*r, sw);

  // i += ring(p2, .2*r, sw);
  // i += ring(-p+v, 1.*r, sw);//lower

  // vec2 v2 = vec2(0., -1.5 + t/2.);
  gl_FragColor = vec4(vec3(i),1.);
}

  // i += ring(p+v2, 1.5*r, sw);
  // i += ring(p+vec2(0., .8)*r, .25*r, sw);
  // i += ring(p+vec2(0., -.8)*r, .25*r, 0.01);