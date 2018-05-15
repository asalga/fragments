precision mediump float;
uniform vec2 u_res;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float len = length(p);
  float i = atan(p.y/p.x)/3.141592658 + 
  			step(len,1.)*pow((1.-len),1./1.3);
  gl_FragColor = vec4(vec3(i),1.);
}