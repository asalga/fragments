// 125 - "Rotate, Alternate"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const float light = 0.8;
const float dark = 0.3;
const vec2 tileSz = vec2(10.);
const float sz = 40.;

mat2 r2d(float a){
	return mat2(cos(a),sin(a),-cos(a), sin(a));
}

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

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


  // 2) 




  // 3) Rotate Tiles


  // 4) 


  // 5)

  float i = ix;

	gl_FragColor = vec4(vec3(i),1);
}