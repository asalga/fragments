// 70 - "Maybe"
precision mediump float;
#define PI 3.141592658

uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
	return length(p)-r;
}

float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}

//from bookofshaders
float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

/*
	state - 0..1
*/
float radio(vec2 p, float rad, float state){
	p.x += rad; // center button

	float button;
	button = step(capsule(p, vec2(0.), vec2(rad*2., 0.), rad), 0.);
	button -= step(capsule(p, vec2(0.), vec2(rad*2., 0.), rad-0.03), 0.);
	
	p.x -= state*rad*2.;
	button += circle(p, rad-0.01); // subtract a tiny bit, otherwise the circle  'peeks' out of the outline.
	button = clamp(button, 0., 1.);
	return button;
}

mat2 r2d(float a){
	return mat2(cos(a),-sin(a),sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time * 1.025;
  float fractTime = fract(t);
  float i;
  float r = 0.25;
  float startX = .25;
  vec2 trans;

  float buttonState = min(mod(t, 2.), 1.);
 
  p *= r2d(PI*step(2., mod(t,4.)));
 
  i += radio(p, r, buttonState);
 
  float inv = step(1.,mod(t,3.));// 0 | 1 
  i = abs(inv - i);

  gl_FragColor = vec4(vec3(i),1.);
}