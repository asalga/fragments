// 22 - Urza's Pendulum
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_lastMouseDown;
#define PI 3.141592658
float cSDF(vec2 p, float r){return length(p) - r;}
void main(){
  float t = u_time * 13.4;
  vec2 a = vec2(1.,u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res * 2. -1.);
  
  vec2 m = vec2(-1.,1.)*a*(u_mouse.xy/u_res*2.-1.);
  vec2 lm = vec2(-1.,1.)*a*(u_lastMouseDown/u_res*2.-1.);

	// When user lets go of the bob, we'll
	// need to figure out the theta 
 float initialTheta = 0.;

	vec2 test = u_lastMouseDown;
  // + vec2(sin(theta), cos(theta));
  // vec2 bPos = p;

  float armLength =length(lm);
  initialTheta = (-atan(lm.y/lm.x));

 if(lm.x > 0.){
  	initialTheta = PI +(atan(-lm.y/lm.x));
  }

	vec2 initialPos = p;// + lm;// + vec2(-1., 1.6);
	vec2 bPos = initialPos;//+ vec2(0., 0.4);// + 0.3 * vec2(sin(t), 0.);

	float theta = sin(initialTheta + t);
	//PI/2. + sin(t) + initialTheta;
	bPos += armLength * vec2(-sin(theta), cos(theta));

  // place bob under cursor if mouse button is down
	if(u_mouse.z == 1.){
		bPos = p+m;
	}

	float i = step(cSDF(bPos, 0.25), 0.);
  gl_FragColor = vec4(vec3(i), 1.);
}