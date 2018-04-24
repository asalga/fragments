// precision mediump float;
// uniform vec2 u_res;
// #define COS_30 0.8660256249

// // return max(a.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;

// // float triangle(vec2 p, float size) {
// //   vec2 a = abs(p);
// //   return max(
//   // a.x * 0.866025 + (p.y * 0.5), 

//   // -p.y * 0.5) 
//   //- size * 0.5;

// float triangleSDF(vec2 p, float s){
//   vec2 a = abs(p);

//   // float yDist = -p.y*.5   +.5;
  
//   // float l = -a.x - yDist;

//   // float r = a.x - yDist;

//   // Take the horizontal distance to the boundry and
//   // scale by cos 30 giving us a diagonal line to the object.
//   float distToSide = a.x * COS_30;

//   //
//   float u = p.y * 0.5;

  
//   // We're offsettting it!
//   // we are offsetting the triangle

//   // Eventually py becomes negative, so we need
//   // to max() it
//   float _1 = distToSide + u;


//   float _2 = -u;

//   // max - for the bottom part. we're preventing 
//   // any fragments from satisfying the case of the 
//   // triangle being taller than desired.
//   float m = max(_1,_2);
//   return m - s;
// }

// void main(){
//   vec2 a = vec2(1., u_res.y/u_res.x);  
//   vec2 p = a * vec2(gl_FragCoord.xy/u_res * 2. -1.);

//   p+= vec2(0.5, -0.5);

//   float i = step(0.,triangleSDF(p, .5));
//   // float i = triangleSDF(p, .5);
//   gl_FragColor = vec4(vec3(i), 1.);
// }

// // float triangle(vec2 p, float size) {
// //   vec2 q = abs(p);
// //   return max(q.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;
// // }


precision mediump float;
uniform vec2 u_res;
uniform float u_time;

// get the scalar distance between p.y and given y
float horizLineSDF(vec2 p, float y, float thick){
  return step(abs(p.y - y), thick);
}

// calculate the length of the vector
// check if inside boundry
float circleSDF(vec2 p, float r){
  return step(length(p), r);
}

// create SDF square visualization

// If the point is inside 'quadrant 1' or 'quadrant 3', we use the 
// x value distance.
//     Q2 
//  Q3    Q1
//     Q4
float squareSDF(vec2 p, float s){
  vec2 a = abs(p);
  return step(max(a.x, a.y), s);
}

vec3 squareSDF_debug(vec2 p, float s){
  vec2 a = abs(p);
  if(a.x - a.y > 0.03){
  	return vec3(0., 0., 1.);
  }
  else if(a.x > a.y){
    return vec3(1., 0., 0.);    
  }
  return vec3(0., 1., 0.);
}

float diamondSDF(vec2 p, float s){
  return 1.;
}

float lens(vec2 p, float r, float n){
  float a = step(length(p+vec2(0.,r))-r, r);
  float b = step(length(p-vec2(0.,r))-r, r);
  return a*b;
}

// float lineSDF(vec2 p, float b, float thick){
//   return 1.;
// }

void main(){
  vec2 a = vec2(1., u_res.y / u_res.x);
  vec2 p = a * ((gl_FragCoord.xy / u_res) *2. -1.);
  float pos = sin(u_time * 1.5);

  float i;

  i = horizLineSDF(p, pos, .005);
  i = circleSDF(p, .5);
  i = squareSDF(p, .5);
  // vec3 d = squareSDF_debug(p, .5);

  gl_FragColor = vec4(vec3(i), 1.);
  // gl_FragColor = vec4(d, 1.);
}



