precision mediump float;
uniform float u_time;
uniform vec2 u_res;
#define PI 3.14159

void main(){
  vec2 ar = vec2(1., u_res.y/u_res.x);
  vec2 p = ar * (gl_FragCoord.xy/u_res*2.-1.);
  
  float density = 1.5;
  float thickness = 0.25;

  float a = atan(p.y,p.x)/3.141592;
  float r = length(p)*density + a/4.;

  float t = mod(u_time/10., 1.);

  float len = length(p);// * mod(a, 1.);

  if(len > 0.45){
  	// discard;
  }

  if(a < .90){
    // discard;
  }

  // r *= t;

  float i = mod(r, .5);
  i = step(thickness, i);
  // i = smoothstep(thickness/20., thickness/30. , i);  

  float red;
  if(r * .5 < 0.30){
     // red = 1.;
  }

  red += step(length(p)- 0.5, 0.);
  
  // gl_FragColor = vec4(vec3(red,i,i),1.);

  vec2 right = vec2(1., 0.);
  vec2 point = normalize(vec2(p));

  float test =  dot(right, point);

  //float angle = atan(p.y,p.x)/PI;

  // float test = 1.;
  float c;
  if(test < .5){
    c = 0.5;
  }

  gl_FragColor = vec4(vec3(c), 1.);
}