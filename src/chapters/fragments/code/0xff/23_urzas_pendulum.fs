// 23 - Urza's Pendulum
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_lastMouseDown;
#define PI 3.141592658
#define TAU PI * 2.
float cSDF(vec2 p, float r){return length(p) - r;}
void main(){
  float t = u_time * 10.;
  vec2 ar = vec2(1.,u_res.y/u_res.x);
  float friction = max(0.,1.-(u_time/2.));
  
  vec2 p = ar*(gl_FragCoord.xy/u_res * 2. -1.);p.y-= 1.;
  
  vec2 m = vec2(-1.,1.)*ar*(u_mouse.xy/u_res*2.-1.);
  vec2 lm = vec2(-1.,1.)*ar*(u_lastMouseDown/u_res*2.-1.);
  lm.y +=1.;m.y +=1.;

  float armLength = length(lm);
  vec2 point = normalize(vec2(abs(lm.x), lm.y));
  vec2 downVec = vec2(0.,1.);
  float d = dot(downVec, point);
  float swingMag = acos(d);

  float theta = cos(t) * swingMag * friction;
	vec2 bPos = p + armLength * (vec2(-sin(theta), cos(theta)));

	if(u_mouse.z == 1. && u_lastMouseDown.x < -10.){
    bPos = p + m;
  }
 
	float i = step(cSDF(bPos, 0.25), 0.);
  i += step(cSDF(p, 0.05), 0.);

  gl_FragColor = vec4(vec3(i), 1.);

  if (abs(p.x) < 0.01 || abs(p.y) < 0.01){
    gl_FragColor = vec4(1.);
  }
}