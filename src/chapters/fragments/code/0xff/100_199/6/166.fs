precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float sdRing(vec2 p, float r, float w){
  return abs(length(p)- (r*.5)) - w;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

mat2 r2d(float a){
  return mat2(cos(a),sin(a), -sin(a), cos(a));
}

float truchet(in vec2 p, float s){

  if(s > 0.75){
    p = vec2(-p.x, p.y);
  }
  else if(s > .5){
    p = vec2(p.x, -p.y);
  }
  else if( s > .25){
    p = -p;
  }

  float t = sin(u_time*0.5);
  //float a = smoothstep(2., 5., t) * PI/2.;
  float a = sin(t)*PI;
  p *= r2d(a);

  float i;
  i += smoothstep(0.1, 0.01, sdRing(p + vec2(1), 2., .02));
  i += smoothstep(0.1, 0.01, sdRing(p - vec2(1), 2., .02));

  return i;
}

float rand(vec2 p){
  vec2 s = vec2(5234.8093, 230.984);
  vec2 r = sin(p*s)*2034.234;
  return fract(r.x*r.y);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float CNT = 4.;
  float i;
  float t = u_time*.2;
  // p += t;

  vec2 id = floor(p*CNT);

  vec2 c = vec2(1./CNT);
  p = mod(p, c)-0.5*c;

  // float i = step(sdCircle(p, 1./CNT), 0.);
  i = truchet(p*CNT*2., rand(id));

  vec2 grid = vec2(p);
  gl_FragColor = vec4(vec3(i),1.);
}