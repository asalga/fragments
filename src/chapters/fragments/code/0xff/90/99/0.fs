precision mediump float;
uniform vec2 res;
uniform sampler2D t0;

float circle(vec2 p, float r){
	return length(p) - r;
}

void main() {

  vec2 as = vec2(1., res.y/res.x);
	vec2 p = as * ((gl_FragCoord.xy /res) *2. -1.);

	float i = step(circle(p, 1.), 0.);

	vec3 col = texture2D(t0, (p+1.)/2. ).rgb;
  gl_FragColor = vec4(vec3(i) * col,1);

	// if(p.x > 0.5){
	// 	gl_FragColor = vec4(1,0,0,1);
	// }
	// else{
	// 	gl_FragColor = vec4(0,0,1,1);
	// }
}