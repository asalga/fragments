precision mediump float;
#define PI 3.14159
uniform vec2 u_res;
uniform float u_time;

float sdCircle(vec2 p, float r){
  return length(p) - r;
}
float circle(vec2 p, float r){
  return 1.-step(0.,sdCircle(p, r));
}
float sdRing(vec2 p, float r, float w){
  return abs(length(p)- r*.5) - w;
}
float ring(vec2 p, float r, float w){
  return smoothstep(0.01, 0.001, sdRing(p,r,w));
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float thickness = 0.005;
  // vec2 p0 = mod(p, 1.);
  // float p0sz = 1.;
  // vec2 p1 = mod(p, 0.5);
  // float p1sz = 0.5;

    // vec2 _p = mod(p,sz);
    // vec2 c = vec2(-sz/2.);
    i += ring(p,2.,thickness);


  // i += ring(p0+vec2(-.5, -.5), p0sz, 0.01);
  // i += ring(p1+vec2(-.5, -.5), p1sz, 0.01);
  for(int it = 1; it < 3; it++){
	float _i = float(it);
  	float sz = 1./_i;
    vec2 _p = mod(p+ vec2(0.,a.y/2.), vec2(sz,0.));
    vec2 c = vec2(-sz/2., -a.y/2.);
    i += ring(_p+c,sz,thickness);
  }

  gl_FragColor = vec4(vec3(i),1);
}