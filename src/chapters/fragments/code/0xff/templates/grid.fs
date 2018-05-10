// Display a 2D array of cells as a graphical aid
precision mediump float;
uniform vec2 u_res;

void main(){
  vec2 p = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(1.);
  vec2 cellSize = vec2(50.);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  gl_FragColor = vec4(vec3(i.x+i.y), 1.);
}