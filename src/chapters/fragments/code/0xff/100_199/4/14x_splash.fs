precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 uv = ar * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time;

  vec2 a = vec2(0., 0.1) * t;
  vec2 v = vec2(0., 0.0) * t;
  v += a;

  vec2 pos = v;

  i += step(sdCircle(uv + pos, 0.25), 0.);

  gl_FragColor = vec4(vec3(i),1.);
}