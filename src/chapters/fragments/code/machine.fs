  // arctan uses the signs of the parameters passed in to determine
  // which quadrant the point is inside
  // atan returns [−π, π]
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159265832

float c(vec2 p, vec2 o, float r){
  return step(length(o-p),r);
}

void main(){
  float i = 0.;
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. - 1.);

  // p.x += 0.5;
  
  vec2 p2 = vec2(p.x+0.5, p.y);
  
  float theta2 = atan(p2.y, p2.x);
  float plen2 = length(p2);

  vec2 p3 = vec2(p.x-0.5, p.y);
  float theta3 = atan(p3.y, p3.x);
  float plen3 = length(p3);

  float theta = atan(p.y, p.x);
  float plen = length(p);
  // i = smoothstep(0.2,0.5, sin((u_time + theta) * 14.)) * 
  //     step(0.4,plen ) * 
  //     smoothstep(plen, plen+0.01, 0.5);


  float f = smoothstep(-0.5, 0.6, cos((theta + u_time) * 20.) ) * .03 + .23;
  i = step(plen, f);

  float f2 = smoothstep(-0.5, 0.6, cos((PI/4. + theta2 - u_time) * 20.)) * .03 + .23;
  float i2 = step(plen2, f2);

  float f3 = smoothstep(-0.5, 0.6, cos((PI/4. +theta3 - u_time) * 20.)) * .03 + .23;
  float i3 = step(plen3, f3);

  gl_FragColor = vec4(vec3(i+i2+i3), 1.);
}

// float f = smoothstep(-.5, 1., cos(theta * 40.)) * .2 + .5;
// float i = 1. - step(f, r);