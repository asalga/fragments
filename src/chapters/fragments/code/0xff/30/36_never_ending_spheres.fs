// Andor
// The magic of repeating a shape is mostly done with the mod
// function as we modulate the coordinates.
//
precision mediump float;
uniform vec2 u_res;

float cSDF(vec2 p, float r){
  return length(p) - r;
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  
  vec2 lineWidthInPx = vec2(.01);
  vec2 cellSize = vec2(.25); 

  // float c = step(cSDF(p, 0.5), 0.);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);

  gl_FragColor = vec4(vec3(i.x+i.y), 1.);
}