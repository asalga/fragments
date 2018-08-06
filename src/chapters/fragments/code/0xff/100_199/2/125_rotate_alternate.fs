precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float light = 0.8;
const float dark = 0.3;
const vec2 tileSz = vec2(10.);
const float sz = 40.;

void main(){
	vec2 p = gl_FragCoord.xy;

	// draw alternating tiles
	// vec2 m = mod(p, tileSz);
  // float i = step(p.xy, m*2.).x;

  // 1) Draw Alternating Tiles
  float my = mod(p.y, sz);
  float iy = step(sz/2., my);
  float mx = mod(p.x+ (iy*sz/2.), sz);
  float ix = step(sz/2., mx);

  float i = ix;

	gl_FragColor = vec4(vec3(i),1);
}