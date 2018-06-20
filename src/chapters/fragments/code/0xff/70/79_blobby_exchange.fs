precision mediump float;

uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU (PI*2.)

float every(float v, float i, float c){
  return mod(v, i) * step(mod(v, c), i);
}

float rectSDF(vec2 p, vec2 size) {//book of shaders
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y),.0)+length(max(d,.0));
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
float ringSDF(vec2 p, float r, float w){
  return abs(length(p) - r * 0.5) - w;
}
void main(){
	const float NUM_BANDS = 3.;

  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time;
	p*=r2d(t*.4);


  // float m2 = .025 /  distance(p+ vec2(-0.5, 0.), vec2(fract(t)*18. - 9., 0.));	  
  // float rect1 = 0.05 / max(rectSDF(p+vec2( .85, 0.), vec2(.25, .02)), 0.);
  // float rect2 = 0.05 / max(rectSDF(p+vec2(-.85, 0.), vec2(.25, .02)), 0.);

	
  float rects = 0.;

  float ring = ringSDF(p, .01, 0.07);
  if(ring < 0.){
  	ring = .0;
  }
  // ring = max(ring, 0.);

  rects += ring;
  for(float theta = 0.; theta < TAU; theta += TAU/4.){
  	p *= r2d(theta);
  	vec2 dims = vec2(.5, .012);
  	vec2 pos = vec2( 1.05, 0.);
		rects += 0.03 / max(rectSDF(p+pos, dims), 0.);
		// rects += .05 / distance(p + vec2(.85, 0.), vec2(0));	
  }

  float balls;
  for(float b=0.;b < 2.;b++){
  	float sc = 5.;

  	vec2 pp = p * r2d(b*PI/2.);
  	float moveTime = t*.5;
  	float time = (fract( moveTime +b/2.)*2.-1.) * sc;
  	float _b = .07 / distance(pp + vec2( 0.5, 0.), vec2(time, 0.));	
  	balls += _b;
  } 

  // float b1 = .025 / distance(p + vec2( 0.5, 0.), vec2(fract(t)*15. - 7., 0.));	
  // float b2 = .025 / distance(p + vec2( 0.5, 0.), vec2(fract(t)*15. - 7., 0.));	

  // float i = 0.2 * (pow(m1, 1.) + pow(m2, 1.));
  // i = .4 * (pow(m1, 2.5) * pow(m2, 3.5));
  
  
  i = balls + rects;
  i = floor(i*NUM_BANDS) * (1./NUM_BANDS);
  i = smoothstep(1., .008, i);
  gl_FragColor = vec4(vec3(i),1.);
}