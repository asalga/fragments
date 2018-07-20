precision mediump float;
uniform float u_time;
uniform vec2 u_res;
uniform vec3 u_mouse;

const float shininess = 20.;
const float lightPower = 1.;
const vec3 diffuseColor = vec3(0.95);
const vec3 ambientColor = vec3(0.2);
const vec3 specColor = vec3(1., 1., 1.);

#define PI 3.141592658

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  vec3 m = vec3(a*(u_mouse.xy/u_res*2.-1.), 1.);  
  float i;
  
  float tileSize = 1.;
  float morterSize = tileSize/10.;

  // p.x += xOffset;

  vec3 l = vec3(0., 0., 1.);
  
  // 0..PI
  float Ix = (mod(p.x, tileSize)/tileSize) * PI;
  float Iy = (mod(p.y, tileSize)/tileSize) * PI;

  float ox = Ix;
  float oy = Iy;

  // float angle = .5;
  // Ix = Ix / PI;
  // Ix *= angle;
  // float delta = 1. - angle;
  // Ix += delta/2.;
  // Ix *= PI;

  //  create cylinders, just to debug
  vec3 n = vec3(-cos(Ix), 0., sin(Ix));


  if(Ix < 0.2 && Ix < 2.8 &&  Iy < 0.2 && Iy < 2.8 ){
    n  = vec3(0. , 0., 0.);
  }
  else if(Iy < 0.1 && Iy < 3. ){
    n  = vec3(0. , 0., 1.);
  }
            // vec3(-cos(Iy), 0., sin(Iy)) , 0.5);








  if(ox > PI/10. && ox < PI - PI/10. && 
    oy > PI/10. && oy < PI - PI/10. ){
  	//n = vec3(0., 0., 1.);
  }
  n = normalize(n);

 
  vec3 lightToFrag = m - vec3(p.x, -p.y, -1.);
  float lightDist = length(lightToFrag);
  lightDist = lightDist*lightDist;// ?????
  lightToFrag = normalize(lightToFrag);

  float xOffset = tileSize * step(tileSize, mod(p.y, tileSize*2.));
  i = step(tileSize, mod(p.x+xOffset, tileSize*2.));
  vec3 tileColor = vec3(i);



  vec3 reflectDir = reflect(lightToFrag, n);
  float specAngle = max(0. ,dot(reflectDir, vec3(0., 0., -1.)));
  float specular =  pow(specAngle, shininess);
  
  vec3 ambientCalc = tileColor * ambientColor;
  vec3 diffuseCalc = diffuseColor * dot(n,lightToFrag) * (lightPower/lightDist);
  vec3 specCalc = specColor * vec3(1.) * specular * (lightPower/lightDist);

  vec3 finalCol =	0. + 
  					// ambientCalc + 
  					diffuseCalc + 
  					specCalc;
  					0.;
  
  gl_FragColor = vec4(finalCol, 1.);
}