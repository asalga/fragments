precision highp float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.;
const float HALF_PI = PI/2.;

const float Rings = 18.;

void r2d(inout vec2 p, float a){
  float s = sin(a);
  float c = cos(a);
  mat2 r = mat2(c,s,-s,c);
  p *= r;
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res*2.-1.;

  float t = u_time * 10.;
  float i;
  float cnt = 2.;

  r2d(p, t/20.);

  for(float it=0.; it < Rings;++it){
    float len = length(p);

    float currRing = it/Rings;

    if(currRing < len){
      vec2 np = p;
      // vec2 tempP = p + vec2(it/10., 0.);
      r2d(np, -sin( (t/10. + currRing)*10. )/ 10.);
      float a = (atan(np.y, np.x)+PI)/TAU * 4.;
      i = smoothstep(0.49, 0.5, mod(a, 1./cnt)* cnt);
    }
  }

  i *= step(length(p), 1.);

  if(length(p)> 0.94){
    i = 0.;
  }
  gl_FragColor = vec4(vec3(i),1);
}