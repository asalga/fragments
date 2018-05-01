precision mediump float;
uniform vec2 u_res;
uniform vec3 u_mouse;
void main(){
  vec2 a=vec2(1.,u_res.y/u_res.x);
  vec2 p=a * (gl_FragCoord.xy/u_res*2.-1.);
  float cnt=u_mouse.x/20.;
  float r=1./(cnt*2.);
  vec2 blCorner=floor(p*cnt)/cnt;
  float l2c=length(p-(blCorner+r));
  float i=1.-smoothstep(r*.98,r,l2c);
  gl_FragColor=vec4(vec3(i),1.);
}