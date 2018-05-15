precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.141592658
#define TAU 2.*PI

float cSDF(vec2 p, float r){
  return length(p)-r;
}
mat2 r2d(float a){
  return mat2(cos(a),-sin(a),sin(a),cos(a));
}


// -PI  ..  PI
// We have values that range from -PI to PI
// we need to divide that into n sections
// find the midpoint
// 



void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  float r = 0.1;
  float count = 4.;
  float sliceSize = TAU/count;





  float i;
  float theta = atan(p.y,p.x);
  float normTheta = ((atan(p.y,p.x)+PI));

  // Here we call cSDF three times
  // In this method, we'll need to call the method for every
  // shape we want to render. Either in a loop or not.
  // i += step(cSDF(p + vec2(cos(PI/2.),sin(PI/2.)), r), 0.);
  // split space into x number of angular sections
  // i += normTheta/count;

  i = mod(normTheta, PI/2.);//*2.-1.;

  // get distance from curr point to closest point
  
  // vec2 v = vec2(1., 0.) * r2d(mod(theta, 0.9));
  float n = count * mod(theta, PI);
  vec2 v = vec2(1., 0.) * r2d(n);

  // if(p.x < 0. && p.y < 0.){
  // 	v = vec2(-.5, -.5);
  // }
  // else if(p.x > 0. && p.y < 0.){
  // 	v = vec2(0.5, -.5);
  // }
  // i = step(cSDF(p - v, 0.4), 0.);

  float tt = (atan(p.y,p.x)+PI)/2./PI;
  float C = 4.;
  float snapTheta = 0.;
  float H = (1./C) /2.;

  // -PI  ..  PI
  float A = atan(p.y, p.x);

  //
  float slice = PI/4.;

  if(A > -PI && A < -PI/2.){
  	float snapTheta = (-3.*PI)/4.;//-2.36
  	v = vec2( cos(snapTheta), sin(snapTheta));
  }

  ////////////////////////
  // 'start' at -PI and increase however much we need
  float T = atan(p.y,p.x);
  float COUNT = 2.;
  float idx = floor(((T+PI)/PI) * COUNT);
  float snapy = -PI + (idx * sliceSize);
  snapy += sliceSize/2.;
  float R = 0.8;
  v = vec2( cos(snapy)*R, sin(snapy)*R);



  // if(tt > 0. && tt < 1./C)  {
  // 	// v = vec2(-.5, -.5);
  // 	snapTheta = 20.;

   
  // 	 //(1./C + (H)) *PI;
  // 	// snapTheta = -1.;
  // 	v = vec2( -cos(snapTheta), sin(snapTheta));
  // }
  // else if(tt > 1./C && tt < 2./C)  {
  // 	v = vec2(.5, -.5);

  // 	snapTheta = -2.;
  // 	//(2./C - (H)) * PI;
  // 	v = vec2( -cos(snapTheta), sin(snapTheta));

  // }
  // else if(tt > 2./C && tt < 3./C)  {
  // 	// v = vec2( .5, .5);
  // 	// snapTheta = 3./C - (H);
  // }
  // else if(tt > 3./C && tt < 4./C)  {
  // 	v = vec2(-.5, .5);
  // }






  // v = vec2( cos(snapTheta), sin(snapTheta));

  // }else{
  // 	v = vec2(-.5, 1.);
  // }
  // else if(p.x > 0. && p.y < 0.){
  // 	v = vec2(0.5, -.5);
  // }
  // i = step(cSDF(p - v, 0.4), 0.);

  i = cSDF(p - v, 0.1);
  // i = step(cSDF(p - v, 0.134), 0.);

   
  // i *= step( acos(dot(p,v)) , 0.9);

  // i += cSDF(p, r);

  // vec2 circP = p * count + 1.;
  // r2d(theta/2.)
  // vec2 circP = p *  5.;
  // circP = mod(circP, vec2(2.)) -1.;

  // vec2 p1 = (p*r2d(0.2) * theta) + vec2(0.5,0.);
  

  // i += step(cSDF(p + vec2(c,s), r), .5);
  // i += step(cSDF(p + vec2(0.), r), 0.);
  // i += step(cSDF(p + vec2(0., +.5), r), 0.);
  // i += step(cSDF(p + vec2(cos(1.),sin(0.)), r), 0.);
  // i += step(cSDF(p + vec2(-cos(1.),sin(0.)), r), 0.);

  // The challenge then is to only call the shape function once



  gl_FragColor = vec4(vec3(i), 1.);
}