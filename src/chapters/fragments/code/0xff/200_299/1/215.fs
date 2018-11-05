precision mediump float;

uniform vec2 u_res;
uniform float u_time;
const float PI = 3.141592658;

float sdCircle(in vec2 p, float r){
  return length(p)-r;
}

void rot(inout vec2 p, float a){
  mat2 m = mat2(cos(a), sin(a),
                -sin(a), cos(a));
  p *= m;
}

void main(){
  vec2 p = (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*0.5;

  float l = length(p);
  float fl = floor(t + l * 10.)/10.;

  // rot(p, t * 0.25 + t * fl);
  rot(p, -t * 0.25 + t * fl);

  float len = .5 - length(p);
  len = pow(len, -0.025) + t;


  float a = (atan(p.y, p.x)+PI)/(PI*2.);


  i = sin(a*PI*20.);
  // i *= floor(l*20.)/20.;
  i -= .01/pow(l, 1.5);

  // i += cos(a*PI*100.);
  // i = pow(i, 1.1);

  // i = step(i,0.);
  // i += smoothstep(0.01, 0.001, sdCircle(p, 0.25));

  gl_FragColor = vec4(vec3(i),1.);
}