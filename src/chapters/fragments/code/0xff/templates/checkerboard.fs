precision mediump float;

const vec2 tileSz = vec2(10.);
const float sz = 40.;

void main(){
	vec2 p = gl_FragCoord.xy;

  float my = mod(p.y, sz);
  float iy = step(sz/2., my);
  float mx = mod(p.x+ (iy*sz/2.), sz);
  float ix = step(sz/2., mx);

  float i = ix;

	gl_FragColor = vec4(vec3(i),1);
}