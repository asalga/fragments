// 58 - "No bounce, no play"
// TODO: bounce to right edge
// add Y direction
// add random positions
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define NUM_BALLS 1
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, length(p) - r);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 metaBalls[NUM_BALLS];
  float i;
  for(int it = 0; it < NUM_BALLS; it++){
	vec2 pos = vec2(u_time, 0.);
    // 8 other "screens" surround the canvas.
    float screenIdx = floor(mod(pos.x, 2.));// 0..1
 	float dir = screenIdx*2.-1.;// remap to  -1..+1
  	vec2 finalPos = vec2(  (1.-screenIdx) + dir * mod(pos.x, 1.), 0.);
	finalPos.x = (finalPos.x-.5) *2.;
    i = circle(p + finalPos, .25);
  }
  gl_FragColor = vec4(vec3(i), 1.);
}