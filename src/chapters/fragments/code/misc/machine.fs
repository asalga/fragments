precision mediump float;
uniform vec2 u_res;
uniform float u_time;
#define PI 3.14159265832
const float NumTeeth = 5.;

float c(vec2 p, vec2 o, float r){
  return step(length(o-p),r);
}

void main(){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 p = a * (gl_FragCoord.xy / u_res * 2. - 1.);
//   float plen = length(p);

//   // 1) Create 'spark pattern'
//   // Get angle formed by current y and x atan will return a value between -pi and pi
//   float theta = atan(p.y, p.x);
  
//   // 1) Feed that into sin which returns a value from -1 to 1
//   float s = sin(theta * NumTeeth);

//   // 2) We can use step to remove the fade created by sin
//   float blade = step(0. ,s);

  

//   float i = blade;
  

//   // 3) We need to 'shape' the ends into teeth
//   float i3 = step(0., sin(theta * NumTeeth));

//   if(plen > 0.4){
//     vec2 z = vec2(p);
//     vec2 test = p * 1.5;
//     z += test;

//     // vec2 t = z * 0.16;
//     // z.y *= -1.;
//     theta = atan(z.y, z.x);


//     float i4 = sin(theta * NumTeeth);

//     float isInside = step(0.0, i4);
    
//     if(isInside > 0.0){
//       i3 = 1.0 - i4;
//     }


//     // i = s;//step(0. ,s);
//   }
//   // i *= step(0.4, plen);
// if(plen > 0.7){discard;}
  
//   // float i2 = smoothstep(-.3, .6, sin(theta * NumTeeth));

//   // scale it down
//   // i2 *= 0.2;

//   // i2 += 0.24;

//   // float i2 = smoothstep(-.9, .9, sin(theta * 10.));// * .08;
//   // i = i * step(plen, 0.6);

//   // float i2 = smoothstep(-.5, .6, sin(theta * 5.)) * 1. + 0.00;

//   // float i2 = step(plen, i3);
//   gl_FragColor = vec4(i3, i3, i3, 1.);


  gl_FragColor = vec4(0., 0., 1., 1.);
}
  
  // float f = smoothstep(-.5, .6, sin(theta * 10.)) * .08;
  // float test = step(0.0001, f);
  //  float i = step(plen, teeth);
  // float i = step(0., v);

    // Image that the sin wave is coming out of the screen
  // Imagine that the sin wave dives into the xz plane
  // and it will make perfect sense.