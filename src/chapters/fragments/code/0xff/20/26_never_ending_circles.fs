precision mediump float;
uniform vec2 u_res;

float cSDF(vec2 p, float r){
  return length(p)-r;
}

// based on fragment, we get the 'closest'
// circle then see if the fragment is inside it.
void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);

  // Problem definition: for the current fragment, 
  // we'll need to find the 'closest' circle
  float r = .1 * 0.5;
  float i = 0.;

  // Find leftmost edge
  // 0 .. 1   
  //           x10
  // 0 .. 10   
  //           floor()
  // 1.1 => 1  , 1.9 = 1 

  // float x = (p.x * 10.)
  // 
  //(floor(p.x/10.)*10.) + (.5*r);

  float leftclosestSideX = floor(p.x * 10.0)/10.0;
  //floor(p.x*2.0) + (0.5*r);
  //vec2(p/10.) + (.5*r);

  float d = p.x - leftclosestSideX;

  //length(p-vec2(leftclosestSideX, p.y));
  // float i = step(0.1, d);

  if(d > 0.01){
  	i = 1.0;
  }

  //length(p - vec2(x, p.y)) - r;
  //float i = step(cSDF(vec2(x,p.y), .2), 0.);
 // float i = step(cSDF(p,r),0.);
  
  gl_FragColor = vec4(vec3(i), 1.);
}