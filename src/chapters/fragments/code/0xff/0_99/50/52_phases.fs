// 52 - "28 phases"
precision mediump float;
#define PI 3.141592658
#define NUM_ROWS 6.
#define NUM_COLS 4.
uniform vec2 u_res;
uniform float u_time;
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - r * 0.5) - w;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);  
  // assign idx to each cell    0 1 2 3 ...
  float xIdx = 4.-floor((p.x+1.)*2.);          // -1..1 > [0..3]
  float yIdx = floor(((p.y+a.y)/(a.y*2.))*7.); // -1.6..1.6 > [0..7]
  float cellIdx = (yIdx *NUM_COLS) + xIdx;
  float idxNormed = cellIdx / (NUM_COLS* NUM_ROWS);
  
  vec2 div = vec2(.5, .42);
  p = mod(p+vec2(0.,.2), div);
  p = (p - div/2.) * vec2(7.);

  float z = sqrt(1. - p.x*p.x - p.y*p.y);
  vec3 normal = normalize(vec3(p.x,p.y,z));
  float t = (u_time * 4.) + (idxNormed*PI);
  vec3 light = vec3(cos(t), 0., sin(t));

  float i = smoothstep(0.07, 0.01, ringSDF(p, 2., 0.01)) + 
  			smoothstep(0.07, 0.01, dot(normal, light));
  gl_FragColor = vec4(vec3(i),1.);
}