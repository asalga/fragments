/*
	Pixelation is fairly straightforward, but there's one
	minor aesthetic issue we need to resolve. If the user
	wants to dynamically change the pixel size, the image will
	get nudged over since sampling always begins with the top
	left pixel.

	Rendering a circle sdf will show the problem.
*/
precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;
// uniform float pxSize;

void main(){
  vec2 p = gl_FragCoord.xy;

  p.y = u_res.y - p.y;

  float pxSize = pow((gl_FragCoord.y/u_res.y)*4., 4.);

  float i = 0.;

    // pxSize = 1. + floor(gl_FragCoord.y/u_res.y*10.) ;

    float y = gl_FragCoord.y;

    // if(y >= 16. * 25.){//400
    //   pxSize = 16.;
    //   // i = 0.15;
    // }
    // else if(y >= 8. * 30.){// 320
    //   pxSize = 8.;
    //   // i = 0.2;
    // }
    // // else if(y >= 4. * 38.){ //152
    //   // pxSize  = 4.;
    //   // i = 0.14;
    // // }
    // else if(y >= 2. * 80.){//40.
    //   pxSize  = 4.;
    // }
    // else{
      // pxSize = 10.+((sin(u_time)+1.)/2.) * 20.;
      pxSize = 1.;
    // }




  // if(y >= 16. * 25.){//400
  //     pxSize = 32.;
  //     // i = 0.15;
  //   }
  //   else if(y >= 8. * 30.){// 320
  //     pxSize = 16.;
  //     // i = 0.2;
  //   }
  //   // else if(y >= 4. * 38.){ //152
  //     // pxSize  = 4.;
  //     // i = 0.14;
  //   // }
  //   else if(y >= 2. * 80.){//40.
  //     pxSize  = 8.;
  //   }
  //   else{
  //     pxSize = 1.;
  //   }


  //   // float py = gl_FragCoord.y/u_res.y;

    // pxSize =  pow(floor(py*10.), 2.)/5.;

    //(float(it)/10.) * p.y/1. * pxSize/1.;

    vec2 c = floor(p/pxSize) * pxSize;

    // Use pixel closest to 'center'. Mostly
    // relevant for larger pxSize values
    c += floor(pxSize/2.);

    // We're already at the center, add one if odd
    c += step(1., mod(pxSize, 3.));

    // c.y *= 5.;
    // if(it == 10){
      // i = texture2D(u_t0, c/u_res).x;
    // }
    // else{



      i += texture2D(u_t0, c/u_res).x;
    // }

    // make it super aliased
    // i = step(i,0.);



  gl_FragColor = vec4(vec3(i),1);
}