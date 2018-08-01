// 75 Shatter/Plasma
precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*12.;

  i += sin(p.x * PI * 5. -t) * sin(u_time+2.);
  i += sin(t+length(p*10.)*PI) * cos(-u_time);
  i += sin((p.x+p.y) * 5. +t) * sin(u_time);

  gl_FragColor = vec4(vec3(i),1.);
}