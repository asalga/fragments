precision mediump float;

uniform sampler2D t0;
uniform vec2 res;

void main() {
	vec2 as = vec2(1., res.y/res.x);
	vec2 p = as * ((gl_FragCoord.xy /res) *2. -1.);
  vec3 col = texture2D(t0, (p+1.)/2. ).rgb;
  
	gl_FragColor = vec4(col + vec3(0.2, 0.4, 0.8), 1.);
}