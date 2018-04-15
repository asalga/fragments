precision mediump float;
uniform vec2 u_res;
uniform float u_time;

float triangleSDF(vec2 p, float s){  
	// Y AXIS TO POINT
  float horizDist = p.x/2. + .5;
  float triangleHorizDist = -(p.y/2.) + .5;
  return 0.8660256249 * (horizDist - triangleHorizDist);

float v = 0.0;
  return 0.8660256249 * (p.x - v);
}

void main(){
  // vec2 a = vec2(1., u_res.y/u_res.x);  
  vec2 p = vec2(gl_FragCoord.xy/u_res * 2. -1.);

  // vec2 p = a * vec2(gl_FragCoord.xy/u_res);

  float i = step(triangleSDF(p, 1.), 0.1);
  // float i = triangleSDF(vec2(-p.x, p.y), 1.);
  gl_FragColor = vec4(vec3(i), 1.);
}