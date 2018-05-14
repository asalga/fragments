  #ifdef RENDER_GRID
    vec2 cellSize = vec2(count/2.); 
    vec2 modp = mod(p, cellSize);
    modp -= cellSize/2.;
    vec2 lineWidthInPx = vec2(0.01);
    vec2 grid = step(mod(p, cellSize), lineWidthInPx);
    i += grid.x + grid.y;
  #endif