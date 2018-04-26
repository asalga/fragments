// 22 - Urza's Pendulum
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_lastMouseDown;
#define PI 3.141592658
#define TAU PI * 2.
float cSDF(vec2 p, float r){return length(p) - r;}
void main(){
  float t = u_time * 5.;
  vec2 ar = vec2(1.,u_res.y/u_res.x);
  vec2 p = ar*(gl_FragCoord.xy/u_res * 2. -1.);
  
  vec2 m = vec2(-1.,1.)*ar*(u_mouse.xy/u_res*2.-1.);
  vec2 lm = vec2(-1.,1.)*ar*(u_lastMouseDown/u_res*2.-1.);

	// When user lets go of the bob, we'll
	// need to figure out the theta 
 //float initialTheta = 0.;

	// vec2 test = u_lastMouseDown;
  // + vec2(sin(theta), cos(theta));
  // vec2 bPos = p;

  float armLength =length(lm);
  // initialTheta = -atan(lm.y/lm.x);
  // initialTheta = (atan(lm.y/lm.x) + PI) /2.;
  float initialTheta = PI-atan(lm.y,lm.x);


  // if(lm.x > 0.){
  	// initialTheta = PI-atan(lm.y,lm.x);
  // }

  // initialTheta += PI/2.;
	//vec2 initialPos = p;// + lm;// + vec2(-1., 1.6);
	//vec2 bPos = initialPos;//+ vec2(0., 0.4);// + 0.3 * vec2(sin(t), 0.);

  float theta = initialTheta;  
  vec2 point = normalize(vec2(abs(lm.x), lm.y));

  vec2 rightVec = vec2(0.,1.);
  float d = dot(rightVec, point);
  float swingMag = acos(d);

  // very top will mean we're pulling the bob 
  // as far up as it will go..
  theta = cos(t) * swingMag;
	vec2 bPos = p + armLength * (vec2(-sin(theta), cos(theta)));

  // place bob under cursor if mouse button is down
	if(u_mouse.z == 1.){
		bPos = p+m;
	}
 
	float i = step(cSDF(bPos, 0.25), 0.);

  gl_FragColor = vec4(i, i ,i, 1.);

  if (abs(p.x) < 0.01 || abs(p.y) < 0.01){
    gl_FragColor = vec4(1.);
  }
}