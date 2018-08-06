// 125 - "Rotate, Alternate"
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

const vec2 tileSz = vec2(40.);
const float sz = 60.;
const float PI = 3.141592658;
// const float TAU = PI*2.;

mat2 r2d(float a){
	return mat2(cos(a),sin(a),-sin(a),cos(a));
}

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

void main(){
	vec2 p = gl_FragCoord.xy;
	float t = u_time*1.;
	float i = 0.;

  vec2 rp = ((mod(p, sz)/sz) * 2.) -1.;
  mat2 r = r2d(t);

  // [0,0] [1,0] [2,0] ...
  // [0,1] [1,1] [2,2] ...
  vec2 cell = floor(p/sz);
  
	// if even, draw rect
	bool isEven = mod(cell.x+ cell.y, 2.) < 1.;
	if(isEven){
		rp *= r;//(cell.x + cell.y) );
  	i = step(sdRect(rp, vec2(1.)), 0.);	
  }
  // if odd, draw 4 surrounding rects
  else{
  	vec2 one = vec2(1.);

  	i += step(sdRect( (rp - vec2( 0,  2.)) * r, one), 0.);	// top
  	i += step(sdRect( (rp - vec2( 0, -2.)) * r, one), 0.);	// bottom
  	i += step(sdRect( (rp - vec2( 2,  0.)) * r, one), 0.);	//
  	i += step(sdRect( (rp - vec2(-2,  0.)) * r, one), 0.);	//
  }

	gl_FragColor = vec4(vec3(i),1);
}