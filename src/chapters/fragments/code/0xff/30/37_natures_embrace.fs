precision mediump float;
#define PI 3.141592658
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i = 0., r = .248;
  float numCircles = 2.;
  vec2 BLCorner = floor(p*numCircles)/numCircles;

  vec2 pToC = p-BLCorner;
  float cartGrid = step(pToC.x, 0.015) + step(pToC.y, 0.015);  
  // i += grid;

  vec2 closestCorner = (floor(p*numCircles)/numCircles)+r;
  float circles = step(length(p-closestCorner),r);
  // i += circles;

  float theta;
  // theta += 0.02;
  theta = (1.-length(p)) + (atan(p.y/p.x)/PI*(.34))*pow(1.,1.);
  float closestAngle = floor(theta*3.)/3.;
  float polarGrid = step(theta-closestAngle, 0.05);
  i += polarGrid;

  //step(pToC.x, 0.015) + step(pToC.y, 0.015); 

  i = abs( step(0.,p.y) -i);
  gl_FragColor = vec4(vec3(i), 1.);
}

  // length(p - vec2(x, p.y)) - r;
  // float i = step(cSDF(vec2(x,p.y), .2), 0.);
  // float i = step(cSDF(p,r),0.);
  // gl_FragColor = vec4(vec3(smoothstep(r/1., r/1.1,len)),1.);
