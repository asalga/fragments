// 6x - "ray trace" method from scratchapixel
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 col;
  float t = u_time;
	float t0, t1; // intersections

  vec3 spherePos = vec3(0., 0., -5.);
  float sphereRad = 1.;

  vec3 rayOrigin = vec3(p.x, p.y, 0.);
  vec3 rayDir = vec3(0., 0, -1.);

  // Need to t0 w/ tca and thc
  vec3 rayToSphere = spherePos - rayOrigin;

  // Projection
  float tca = dot(rayToSphere, rayDir);

  if(tca < 0.){
  	col.r = 0.5;
  }


  gl_FragColor = vec4(col,1.);
}