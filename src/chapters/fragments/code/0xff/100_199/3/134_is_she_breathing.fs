// 134 - Is She Breathing?
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float PI = 3.141592658;
const float TAU = PI*2.0;
const float E = 0.001;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRing(vec2 p, float r1, float r2){
  float u = 0.0;
  float l = 0.01;
  const float D = 0.005;

  float c = smoothstep(l, u, sdCircle(p, r1)) -
            smoothstep(l-D, u-D, sdCircle(p, r2));
  return c * smoothstep(1., 0., r1*2.);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res * 2. -1.;
  float i;
  float t = u_time*2.;

  float r = 0.0;

  float len = 0.5;
  const float NumDrops = 10.;
  const float inc = TAU/NumDrops;

  for(float it = 0.; it < NumDrops; it += inc){
    float theta = TAU/NumDrops * it;
    vec2 v = vec2(cos(theta)*len, sin(theta)*len);
    r = fract( (t + (it*1.)) / 10.);
    i += sdRing(p+v, r, r - E);
  }

  gl_FragColor = vec4(vec3(i), 1.);
}