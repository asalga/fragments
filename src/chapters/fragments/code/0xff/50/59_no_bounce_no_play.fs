// 58 - "No bounce, no play"
precision mediump float;
uniform vec2 u_res;
#define PI 3.141592658
uniform float u_time;
#define NUM_BALLS 10
highp float rand(vec2 co){// from byteblacksmith.com
  highp float a = 12.9898;
  highp float b = 78.233;
  highp float c = 43758.5453;
  highp float dt= dot(co.xy ,vec2(a,b));
  highp float sn= mod(dt,3.14);
  return fract(sin(sn) * c);
}
// float sdCircle(vec2 p, float r){
  // return length(p) - r;
// }
float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, length(p) - r);
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 metaBalls[NUM_BALLS];
  float i;
  for(int it = 0; it < NUM_BALLS; it++){
  	float f_it = float(it);
    vec2 ballVel = vec2(rand(vec2(f_it+1.734))*2.-1.,
    					rand(vec2(f_it-1.734))*2.-1.);
	vec2 pos = vec2(0.5) + ballVel * vec2(u_time);
    // float screenIdx = floor(mod(pos.x, 2.));// 0..1
    vec2 screenIdx = floor(mod(pos, vec2(2.)));// 0..1
 	vec2 dir = screenIdx*2.-1.;// remap to  -1..+1
  	vec2 finalPos = vec2((1.-screenIdx) + dir * mod(pos, 1.));
	finalPos = p + (finalPos-.5)* 2.;
    i += circle(finalPos, .25);
  }
  i = 1. - step(mod(i, 2.), 0.);// invert on odd/even
  gl_FragColor = vec4(vec3(i), 1.);
}