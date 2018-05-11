precision mediump float;
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  p += vec2(u_time/1. - p.y, -u_time/2. + .25);
  float pxf = floor(p.x*4.);
  float b = step(-pxf,floor(p.y*8.));
  float t = 1.-step(floor((p.y-.5)*8.),-pxf);
  float i = step(mod(p.x,0.5),.25) * b + t;
  gl_FragColor = vec4(vec3(i),1.);
}