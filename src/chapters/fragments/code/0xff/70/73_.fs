precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}
float square(vec2 p, float halfW){
	return step(sdSquare(p, halfW), 0.);
}
float sdCircle(vec2 p, float r){
  return length(p)-r;
}
float circle(vec2 p, float r){
	return step(sdCircle(p,r), 0.);
}
mat2 r2d(float a){
	return mat2(cos(a),-sin(a),sin(a),cos(a));
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float time = u_time;
 	mat2 r = r2d(time*.1);

  // vec2 squarePos = p + vec2(0., sin(time*0.));//* r2d(time*1.);
  vec2 squarePos = p * r;
 
	vec2 v = vec2(0.25, -0.25);
  vec2 c0 = p + v;

  float dist = sdSquare( (v*r), 0.25);
  i += circle(c0 + vec2(0., -dist*2.), 0.02);

  i += square(squarePos, 0.25);

  gl_FragColor = vec4(vec3(i),1.);
}