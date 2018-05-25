// 52 - phases
precision mediump float;
#define SZ 1.
uniform vec2 u_res;
uniform float u_time;
float grid(){vec2 p = gl_FragCoord.xy;vec2 lineWidthInPx = vec2(1.);vec2 cellSize = vec2(75.);vec2 i = step(mod(p, cellSize), lineWidthInPx);return i.x + i.y;}
float cSDF(vec2 p, float r ){
  return length(p) - r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*2.;
  vec3 light = vec3(cos(t), 0., sin(t));
  float i;

  // vec2 sz = vec2(.5, .42);
  // p = mod(p+vec2(0.,.2), sz);
  // p -= sz/2.;

  // assign idx to each cell
  // 0 1 2 3 ...
  float numCols = 4.;
  float xIdx = floor((p.x+1.)*2.); // -1..1 > [0..3]
  float yIdx = floor(((p.y+a.y)/3.2)*6.); // -1.6..1.6 > [0..6]
  float cellIdx = (yIdx *numCols) + xIdx;

  float percent = cellIdx/ (4.*6.);
  
  // float phase = idx / totalCells;
  // get percentage of idx to cell
  // 0..24
  // convert to 0..1 range
  // /24
  // add phaseOffest to calculated offset

  vec2 sz = vec2(.5);
  p = mod(p, sz);
  p -= sz/2.;
  p *= vec2(4.);
  
  // calc normal
  float z = sqrt(1.-p.x*p.x - p.y*p.y);
  vec3 n = vec3(p.x,p.y,z);
  n = normalize(n);

  float intensity = dot(n, light);
  intensity = step(intensity, 0.);

  // i = step(cSDF(p , SZ/2.), 0.) * intensity;
  i = intensity;
  i += grid();
  gl_FragColor = vec4(i,i,i,1.);
}