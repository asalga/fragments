precision mediump float;
uniform vec2 u_res;
uniform vec3 u_mouse;
#define CNT 40
#define COS_30 0.866
float random (vec2 st) {
  return fract(sin(dot(st.xy,vec2(12.9898,78.233)))*43758.5453123);
}
float rSDF(vec2 p, vec2 size){
  vec2 absValue = abs(p / size);
  float side = max(absValue.x, absValue.y);
  return step(size.y, side);
}
float cSDF(vec2 p, float r){return length(p)-r;}
float cInter(vec2 p, float r, float i){
  float a = smoothstep(0.01, 0.001,cSDF(p+vec2(-i,.0), r));
  float b = smoothstep(0.01, 0.001,cSDF(p+vec2(i,0.), r));
  return a*b;
}
float window(vec2 p){
  float rect = step(rSDF(p+vec2(0., .4), vec2(0.6, .9)), 0.);
  float cInt = cInter(p+vec2(0., -.45), .79, 0.25);
  return clamp((rect + cInt), 0., 1. );
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec2 m = a*(u_mouse.xy/u_res*2.-1.);
  m.x *=- 1.;

  vec3 c;
  vec3 test[CNT];
  vec3 colors[CNT];

  for(int i = 0; i < CNT; i++){
  	test[i] = vec3( 2.*fract(sin(float(i)*1000.0))-1.,
  					2.*fract(sin(float(i)*20000.0))-1.,
  					2.*fract(cos(float(i)*10000.0))-1.);

  	colors[i] = vec3(random(vec2(float(i*i))),random(vec2(float(i*i*3))),random(vec2(float(i))));
  }

  int closestIdx = -1;
  float dist = length(vec2(-m.x, -m.y) -p);
  for(int i = 0; i < CNT; i++){
  	float testLength = length(p-vec2(test[i]));
    if(testLength <  dist){
    	closestIdx = i;
    	dist = testLength;
    	c = colors[i];
    }
  }
  if(closestIdx == -1){
  	c = vec3(.3,.6,.9);
  }
  vec3 windowoutline = vec3(window(p*vec2(.95, 0.95)));
  vec3 windowin = vec3(window(p));
  vec3 outline = (windowoutline-windowin);
  vec3 final = c * windowin + outline;
  gl_FragColor = vec4(final,1.);
}