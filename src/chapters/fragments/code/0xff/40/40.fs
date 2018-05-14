precision mediump float;
uniform vec2 u_res;
#define PI 3.14159

float cSDF(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float r = 0.1;
  float count = 10.;

  float i;
  float theta = (atan(p.y,p.x));
  float normTheta = ((atan(p.y,p.x)/PI)+1.)/2.;

  // Here we call cSDF three times
  // In this method, we'll need to call the method for every
  // shape we want to render. Either in a loop or not.
  // i += step(cSDF(p + vec2(cos(PI/2.),sin(PI/2.)), r), 0.);

  float c = cos(theta) * .54;
  float s = sin(theta) * .54;

  vec2 circP = p * count + 1.;
  circP = mod(circP, 2.)-1.;

  // vec2 p1 = (p*r2d(0.2) * theta) + vec2(0.5,0.);
  i += step(cSDF(circP, r), .25);

  // i += step(cSDF(p + vec2(c,s), r), .5);

  // i += step(cSDF(p + vec2(0.), r), 0.);

  // i += step(cSDF(p + vec2(0., +.5), r), 0.);
  // i += step(cSDF(p + vec2(cos(1.),sin(0.)), r), 0.);
  // i += step(cSDF(p + vec2(-cos(1.),sin(0.)), r), 0.);

  // The challenge then is to only call the shape function once



  gl_FragColor = vec4(vec3(i), 1.);
}