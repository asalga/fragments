precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
float c(vec2 p, float r){
  vec2 a = vec2(u_res.x/u_res.y, 1.);  
  vec2 uv = a * ((gl_FragCoord.xy / u_res) * 2. - 1.);
  float t = r * a.x;
  return 1. - smoothstep(t, t + 0.015, distance(p, uv));
}

void main(){
  vec2 a = vec2(u_res.x/u_res.y, 1.);   
  vec2 p = a * ((gl_FragCoord.xy / u_res) * 2. - 1.);
  p = normalize(p);
  float th = acos(dot(vec2(0., 1.), p)) / PI/2.;
  float g = step(.0, p.x);
  float t = g * 2. - 1.;
  th = g - t * th;
  float i = c(vec2(0.), .9) - c(vec2(0.), .9 - .06);
  i *= step(th, u_time);
  i += c(vec2(0.), .1);
  i += c(vec2(0., 0.548), .1) * step(1.0, u_time);
  gl_FragColor = vec4(vec3(i), 1.);
}