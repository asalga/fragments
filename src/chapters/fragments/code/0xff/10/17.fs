precision mediump float;
uniform vec2 u_res;
uniform float u_time;
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  vec3 ldir = vec3(cos(u_time), .0, sin(u_time));
  float z = sqrt(1.-p.x*p.x-p.y*p.y);
  vec3 n = normalize(vec3(p, z));
  vec3 xzVec = normalize(vec3(n.x, 0., n.z));
  float anY = (atan(xzVec.x/xzVec.z)/3.1415)*2. + u_time;
  float h = .25 * step(mod(p.y, .4), .2);
  float i = step(1./4., mod(anY + h,.5)) * step(length(p), 1.);
  gl_FragColor = vec4(vec3(i),1.);
}