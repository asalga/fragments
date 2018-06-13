// 70 - "Maybe"
precision mediump float;
#define PI 3.141592658
#define TAU 2.*PI
#define NUM_BTNS 8

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
	button = smoothstep(0.03, 0.0001, capsule(p, vec2(0.), vec2(rad*2., 0.), rad));
	button -= smoothstep(0.03, 0.0001, capsule(p, vec2(0.), vec2(rad*2., 0.), rad-0.03));
	
	p.x -= state*rad*2.;
	button += circle(p, rad-0.01); // subtract a tiny bit, otherwise the circle  'peeks' out of the outline.
	button = clamp(button, 0., 1.);

	if(state == 1.){
		button -= circle(p, rad-0.04);
	}

	return button;
}

mat2 r2d(float a){
	return mat2(cos(a),-sin(a),sin(a),cos(a));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time * 2.;
  float fractTime = fract(t);
  float i;
  float r = 0.25;
  float isLightOn;

  p *= r2d(PI/2.);

  for(int it = 0; it < NUM_BTNS; ++it){
  	vec2 _p = p* 3.;// scale down

  	float an = float(it)/float(NUM_BTNS) * TAU;
  	_p *= r2d(an);
  	_p += vec2(1.5, 0.);
  	
  	float _t = t + an/5.;
  	float buttonPos = min(mod(_t, 2.), 1.);
		float buttonState = step(1.,mod(_t,4.)) * step(mod(_t,4.), 3.);
 		mat2 rot1 = r2d(PI*step(2., mod(_t,4.)));

  	i += radio(_p*rot1, r, buttonPos);
  	isLightOn += buttonState;
  }

  float innerCircleSub = circle(p, 0.2 * (isLightOn/float(NUM_BTNS)+.1));
  float innerCircle = circle(p, 0.25);
  
  i +=(innerCircle - innerCircleSub);
  gl_FragColor = vec4(vec3(i,i,i),1.);
}