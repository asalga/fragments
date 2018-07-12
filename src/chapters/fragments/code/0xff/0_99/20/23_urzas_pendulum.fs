// 23 - Urza's Pendulum
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform vec2 u_lastMouseDown;
#define PI 3.141592658
#define TAU PI * 2.
// capsule definition source from book of shaders
float capsule(vec2 p, vec2 a, vec2 b, float r) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
  return length(pa-ba*h)-r;
}
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
  // we already havce the length, no point in calling 
  // normlized which calcs len again.
  vec2 point = lm/armLength;
  vec2 downVec = vec2(0.,1.);
  float d = dot(downVec, point);
  float swingMag = -acos(d);
  
  // depending on where user clicked. left or right side.
  swingMag *= step(0.0, lm.x)*2. -1.;
  
  float theta =  cos(t) * swingMag * friction;
	vec2 bPos = p + armLength * (vec2(-sin(theta), cos(theta)));

	if(u_mouse.z == 1.){
    bPos = p + m;
  }

	float i = smoothstep(0.01, 0.001, cSDF(bPos, 0.25)) -
            2.*smoothstep(0.01, 0.001, cSDF(bPos, 0.24)) +
            2.*smoothstep(0.01, 0.001, cSDF(bPos, 0.23)) +
            smoothstep(0.01, 0.001, cSDF(p, 0.05)) + 
            smoothstep(0.01, 0.001, capsule(p, vec2(0.), p - bPos, 0.005));

  gl_FragColor = vec4(vec3(i), 1.);
}