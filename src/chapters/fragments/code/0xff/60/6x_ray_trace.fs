// 6x - "ray trace" method from scratchapixel
precision mediump float;
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 col;
	float t0, t1; // intersections

  vec3 spherePos = vec3(0., 0., -5.);
  float rad = 1.;

  vec3 rayOrigin = vec3(p.x, p.y, 0.);
  vec3 rayDir = vec3(0., 0, -1.);

  // Need to t0 w/ tca and thc
  vec3 rayToSphere = spherePos - rayOrigin;

  // Project vector rayToSphere onto rayDir
  float tca = dot(rayToSphere, rayDir);

  // if negative, no intersection
  // rayDirection was pointing in opposite direction
  if(tca < 0.){col.r = 0.5;}

  // d = sqrt(rayToSphere² - tca²)
  float d = sqrt(dot(rayToSphere,rayToSphere) - dot(tca,tca));	

  // thc = sqrt(r² - d²)
	float thc = sqrt(pow(rad,2.) - pow(d,2.));


  gl_FragColor = vec4(col,1.);
}