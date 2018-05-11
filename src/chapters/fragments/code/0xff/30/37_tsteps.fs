precision mediump float;
uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  p.x += u_time;
  p.x -= 1. * p.y;//shear
  p.y -= u_time/2.;
  p.y += 0.25;//nudge down

  float pxf = floor(p.x*4.);
  
  float bottom = step(-pxf,floor(p.y*8.));
  float top = 1.-step(floor((p.y-.5)*8.),-pxf);
  
  float i = step(mod(p.x,0.25),.125) * bottom;
  
  i += top;
  gl_FragColor = vec4(vec3(i),1.);
}