precision mediump float;
uniform vec2 u_res;
#define COS_30 0.8660256249

float triangleSDF(vec2 p, float s){  
  float yDist = -p.y/2.+.5;
  
  float l = -p.x - yDist;
  float r = p.x - yDist;
  
  return (l*COS_30) * (r*COS_30);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);  
  vec2 p = a * vec2(gl_FragCoord.xy/u_res * 2. -1.);

  
  float i = 1. - step(triangleSDF(p, 1.), 0.0);
  gl_FragColor = vec4(vec3(i), 1.);
}