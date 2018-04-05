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