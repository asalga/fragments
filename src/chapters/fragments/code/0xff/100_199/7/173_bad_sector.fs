precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI/2.;

const float Rings = 8.;

void r2d(inout vec2 p, float a){
  float s = sin(a);
  float c = cos(a);
  mat2 r = mat2(c,s,-s,c);
  p *= r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;
  float CNT = 6.;
  float t = u_time;
  float i;
  float cnt = 4.;

  for(float it=0.; it < Rings;++it){
    float len = length(p);

    float currRing = it/Rings;

    if(currRing < len){
      r2d(p, t/4.);
      float a = (atan(p.y, p.x)+PI)/TAU;
      i = smoothstep(0.49, 0.5, mod(a, 1./cnt)* cnt);
    }
  }

  i *= step(length(p), 1.);

  gl_FragColor = vec4(vec3(i),1);
}