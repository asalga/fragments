precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float cSDF(vec2 p, float r){
  return length(p)-r;
}
float opSub(float a, float b){
  return a-b;
}

// position, radius, phase
float moon(vec2 p, float r, float phase){
  float a = step(cSDF(p,r), 0.);
  float b = step(cSDF(p+vec2(phase*r*2., 0.),r), 0.);
  return 1.-smoothstep(0.1, 0.01, a-b);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float t = u_time*5.;
  float i;
  vec2 moonp = p;

  // moon needs to have less of a distortion
  moonp.x +=step(p.y, 0.) * .009 * 
  		 	sin(t + p.y * 100.);

  p.x += // only change bottom half 
  		 step(p.y, 0.) * .05 * 
  		 // add waves
  		 sin(t + p.y * 100.) *
  		 // larger waves farther down
  		 1. * abs(p.y);

  // float i = step(cSDF(p, 0.8),.0);
  i = smoothstep(.03,.01, cSDF(p,.8));
  i -= moon(moonp+vec2(0., 0.5), 0.2, .3);
  
  // better way of doing this?
  i = abs(step(0., p.y)-i);
  
  gl_FragColor = vec4(vec3(i), 1.);
}