precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658

float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}


float circleSDF(vec2 p, float r){
  return length(p)-r;
}
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  p += vec2(-.4, -.8);
  float i = 1.-smoothstep(.23, .24, circleSDF(p,.3));
  // float i = 0.;
  
  float theta = atan(p.y,  p.x);

  //theta 0..TAU
  theta *= .1 * (1.+(cos(theta))) + cos(3.*sin(theta)) - cos(-4.*sin(theta));
  // theta *= cos(theta);

  float theta2 = 20.* (1.+sin(atan(p.y, p.x)));

  //theta2 = random(vec2(theta2));
	//theta += theta2;
  
  i += smoothstep(0.,.1,	
  	sin( theta  * 10.)  );

  gl_FragColor = vec4(vec3(1.-i), 1.);
}