float grid(){
  vec2 p = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(1.);
  vec2 cellSize = vec2(50.);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  return i.x + i.y;
}
