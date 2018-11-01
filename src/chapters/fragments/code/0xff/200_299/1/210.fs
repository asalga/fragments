precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

// pos, dim, col
float px(vec2 _, vec2 p, vec2 d, float c){
  return (c/255.) * step(sdRect(_ - p- vec2(d/2.) , d/2. ), 0.);
}


void main(){
  float t = u_time;
  vec2 p = gl_FragCoord.xy;///u_res;
  float i = 0.1;
  p *= 0.032;

  float x = -2. + floor((sin(t*4.)+1.5));
  float y = floor((cos(t*4.)+1.));

  vec2 np = gl_FragCoord.xy/u_res*2.-1.;


  // p -= fract(length(np/2.)+t);

  // p.x += cos(p.y*p.y + t)/2.;

  // 0
  i += px(p, vec2(6, 15), vec2( 4., 1.), 160.);

  // // 1
  i += px(p, vec2(4, 14), vec2( 8., 1.), 160.);

  // // 2
  i += px(p, vec2(3, 13), vec2( 10., 1.), 160.);
  // i += px(p, vec2(8, 13), vec2( 4., 1.), 92.);

  // // 3
  i += px(p, vec2(2, 12), vec2( 12., 1.), 160.);
  i += px(p, vec2(5, 12), vec2( 2., 1.), 64.); // eye 1
  i += px(p, vec2(11, 12), vec2( 2., 1.), 64.);// eye 1

  // 4
  i += px(p, vec2(2, 11), vec2( 12., 1.), 160.);
  i += px(p, vec2(4, 11), vec2( 4., 1.), 64.);// eye 2
  i += px(p, vec2(10, 11), vec2( 4., 1.), 64.);

  // 5
  i += px(p, vec2(2, 10), vec2( 12., 1.), 160.);
  i += px(p, vec2(4, 10), vec2( 4., 1.), 64.);//eye3
  i += px(p, vec2(10, 10), vec2( 4., 1.), 64.);

  i -= 10. * px(p, vec2(6.+x, 10. +y ), vec2( 2., 1.), 64.);// pupil
  i -= 10. * px(p, vec2(12.+x, 10.+y), vec2( 2., 1.), 64.);// pupil


  // 6
  i += px(p, vec2(1, 9), vec2( 14., 1.), 160.);
  i += px(p, vec2(4, 9), vec2( 4., 1.), 64.);//eye4
  i += px(p, vec2(10, 9), vec2( 4., 1.), 64.);
  i -= 10. * px(p, vec2(6. +x, 9. +y  ), vec2( 2., 1.), 64.);// pupil
  i -= 10. * px(p, vec2(12.+x, 9. +y ), vec2( 2., 1.), 64.);// pupil


  // 7
  i += px(p, vec2(1, 8), vec2( 14., 1.), 160.);
  i += px(p, vec2(5, 8), vec2( 2., 1.), 64.);//eye5
  i += px(p, vec2(11, 8), vec2( 2., 1.), 64.);


  // i += px(p, vec2(2, 8), vec2( 5., 1.), 92.);
  // i += px(p, vec2(7, 8), vec2( 5., 1.), 160.);
  // i += px(p, vec2(12, 8),vec2( 2., 1.), 92.);
  // i += px(p, vec2(14, 8), vec2( 2., 1.), 160.);

  // 8
  i += px(p, vec2(1, 7), vec2( 14., 1.), 160.);
  // i += px(p, vec2(2, 7), vec2( 5., 1.), 92.);
  // i += px(p, vec2(7, 7), vec2( 5., 1.), 160.);
  // i += px(p, vec2(12, 7), vec2( 3., 1.), 93.);
  // i += px(p, vec2(15, 7), vec2( 1., 1.), 160.);

  // 9
  i += px(p, vec2(1, 6), vec2( 14., 1.), 160.);


  // 10
  i += px(p, vec2(1, 5), vec2( 14., 1.), 160.);

  // 11
  i += px(p, vec2(1, 4), vec2( 14., 1.), 160.);

  // 12
  i += px(p, vec2(1, 3), vec2( 14., 1.), 160.);

  // 13
  i += px(p, vec2(1, 2), vec2( 14., 1.), 160.);

    // 14
    i += px(p, vec2(1, 1), vec2( 2., 1.), 160.);
    i += px(p, vec2(4, 1), vec2( 3., 1.), 160.);
    i += px(p, vec2(9, 1), vec2( 3., 1.), 160.);
    i += px(p, vec2(13, 1), vec2( 2., 1.), 160.);

    // 15
    i += px(p, vec2(1, 0), vec2( 1., 1.), 160.);
    i += px(p, vec2(5, 0), vec2( 2., 1.), 160.);
    i += px(p, vec2(9, 0), vec2( 2., 1.), 160.);
    i += px(p, vec2(14, 0), vec2( 1., 1.), 160.);
  // }
  // else{
  //   // 14
  //   i += px(p, vec2(1, 1), vec2( 2., 1.), 160.);
  //   i += px(p, vec2(5, 1), vec2( 3., 1.), 160.);
  //   i += px(p, vec2(9, 1), vec2( 3., 1.), 160.);
  //   i += px(p, vec2(14, 1), vec2( 1., 1.), 160.);

    // 15
    // i += px(p, vec2(1, 0), vec2( 1., 1.), 160.);
    // i += px(p, vec2(5, 0), vec2( 2., 1.), 160.);
    // i += px(p, vec2(9, 0), vec2( 2., 1.), 160.);
    // i += px(p, vec2(14, 0), vec2( 1., 1.), 160.);
  // }

  // i += px(p, vec2(0, 5), vec2( 16., 1.), 160.);
  // i += px(p, vec2(1, 4), vec2( 1., 1.), 160.);
  // i += px(p, vec2(2, 4), vec2( 3., 1.), 92.);
  // i += px(p, vec2(5, 4), vec2( 6., 1.), 255.);
  // i += px(p, vec2(11, 4), vec2(3., 1.), 92.);
  // i += px(p, vec2(14, 4), vec2( 1., 1.), 160.);

  // i += px(p, vec2(4, 3), vec2( 8., 1.), 255.);

  // // 13
  // i += px(p, vec2(4, 2), vec2( 6., 1.), 255.);
  // i += px(p, vec2(10, 2), vec2( 1., 1.), 160.);
  // i += px(p, vec2(11, 2), vec2( 1., 1.), 255.);

  // // 14
  // i += px(p, vec2(4, 1), vec2( 6., 1.), 255.);
  // i += px(p, vec2(10, 1), vec2( 1., 1.), 160.);
  // i += px(p, vec2(11, 1), vec2( 1., 1.), 255.);


  // // 15
  // i += px(p, vec2(5, 0), vec2( 4., 1.), 255.);
  // i += px(p, vec2(9, 0), vec2( 1., 1.), 160.);
  // i += px(p, vec2(10, 0), vec2( 1., 1.), 255.);

  // i += fract(i+t*8.);

  // i += fract(length(np)-t/1.)/4.;
  // i /= fract(length(np)-t/1.)*5.;



  gl_FragColor = vec4(vec3(i),1);
}

















