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
uniform float u_pixelSize;

void main(){
  vec2 p = gl_FragCoord.xy;
  vec2 c = floor(p/u_pixelSize)*u_pixelSize;

  // Use pixel closest to 'center'. Mostly
  // relevant for larger u_pixelSize values
  c += floor(u_pixelSize/2.);

  // We're already at the center, add one if odd
  c += step(1., mod(u_pixelSize, 3.));

  float i = texture2D(u_t0, c/u_res).x;
  gl_FragColor = vec4(vec3(i),1);
}