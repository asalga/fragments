precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}

void main(){
	const float NUM_BANDS = 3.;

  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  
  float x = sin(u_time*1.) / 1.5;

  float m1 = .05 / distance(p, vec2(0.15+x, 0.));	
  float m2 = .05  / distance(p, vec2( x, .3));
  float test = 0.03 / max(rectSDF(p-vec2(-.8, 0.), vec2(0.8, 0.1)), 0.);

  // float i = 0.2 * (pow(m1, 1.) + pow(m2, 1.));
  // i = .4 * (pow(m1, 2.5) * pow(m2, 3.5));
  
  i = m1 + test;
  // i = fract(i*100.);
  i = floor(i*NUM_BANDS) * (1./NUM_BANDS);

	//smoothstep(0.7, 0.72,c)

  gl_FragColor = vec4(vec3(i),1.);
}

