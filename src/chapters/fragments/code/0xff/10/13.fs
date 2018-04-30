precision mediump float;
uniform vec2 u_res;
#define X(p,o)(step(d/2., mod(p + o, d))) 
void main() {
  vec2 p = gl_FragCoord.xy;
  float d = u_res.x/(8./2.);
  float i = X(p.x, (X(p.y, 0.) * d/2.));
  gl_FragColor = vec4(i,i,i,1.);
}