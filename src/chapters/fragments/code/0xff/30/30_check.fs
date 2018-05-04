precision mediump float;
uniform vec2 u_res;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float i = 1.;

  float tileSize = 0.5;
  float morterSize = tileSize/10.;

  float xOffset = tileSize * step(tileSize, mod(p.y, tileSize*2.));
  p.x += xOffset;

  i = step(tileSize, mod(p.x, tileSize*2.));
  
  gl_FragColor = vec4(vec3(i), 1.);
}