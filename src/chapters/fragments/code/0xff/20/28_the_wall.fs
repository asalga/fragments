precision mediump float;
uniform vec2 u_res;
uniform sampler2D u_texture0;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  
  float i = 1.;
  vec4 col = texture2D(u_texture0, p);


  gl_FragColor = vec4(vec3(col), 1.);
}