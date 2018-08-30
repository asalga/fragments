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

  float pxSize = 1.;// + pow((gl_FragCoord.y/u_res.y)*4., 4.);

  float i = 0.;

  // for(int it = 10; it > 1; it--){

    // pxSize = 1. + floor(gl_FragCoord.y/u_res.y*10.) ;

    float py = gl_FragCoord.y/u_res.y;

    pxSize =  pow(floor(py*10.), 2.)/5.;

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
      i = texture2D(u_t0, c/u_res).x;
    // }



  gl_FragColor = vec4(vec3(i),1);
}