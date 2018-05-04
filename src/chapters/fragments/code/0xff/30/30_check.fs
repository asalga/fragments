precision mediump float;
uniform float u_time;
uniform vec2 u_res;
uniform vec3 u_mouse;

const float shininess = 40.;
const float lightPower = .8;

#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);  
  
  vec3 ambientColor = vec3(0.1);
  float i;

  vec3 m = vec3(a*(u_mouse.xy/u_res*2.-1.), 1.);  

  float tileSize = 0.125;
  float morterSize = tileSize/10.;

  float xOffset = tileSize * step(tileSize, mod(p.y, tileSize*2.));
  p.x += xOffset;

  vec3 l = vec3(0., 0., 1.);

  //  create cylinders, just to debug
  float I = (mod(p.x, tileSize)/tileSize) * PI;
  vec3 n = vec3(-cos(I), 0., sin(I));


 
  vec3 lightToFrag = m - vec3(p.x, -p.y, -1.);
  float lightDist = length(lightToFrag);
  lightDist = lightDist*lightDist;// ?????
  lightToFrag = normalize(lightToFrag);

  i = step(tileSize, mod(p.x, tileSize*2.));


  vec3 tileColor = vec3(i);

  vec3 reflectDir = reflect(lightToFrag, n);
  float specAngle = dot(reflectDir, vec3(0., 0., -1.));
  float specular = pow(specAngle, shininess*5.0);
  
  vec3 ambientCalc = tileColor * ambientColor;
  float diffuseCalc = dot(n,lightToFrag) * (lightPower/lightDist/4.);
  vec3 specCalc = vec3(1.) * specular * (lightPower/lightDist);

  vec3 finalCol =	ambientCalc + 
  					 diffuseCalc + 
  					specCalc;
  					0.;
  
  gl_FragColor = vec4(finalCol, 1.);
}