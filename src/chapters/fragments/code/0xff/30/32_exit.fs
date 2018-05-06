precision mediump float;
uniform vec2 u_res;
void main(){
  vec2 p = gl_FragCoord.xy;
  float pxSz = 2.;
  float xOffset = pxSz*step(pxSz,mod(p.y,pxSz*2.));
  float i = step(pxSz,mod(p.x+xOffset,pxSz*2.)) * 
        	step(p.y,  p.x) * step(p.y, 200.) + 
        	step(200., p.x) * step(p.y, 200.);
  gl_FragColor = vec4(vec3(i),1.);
}