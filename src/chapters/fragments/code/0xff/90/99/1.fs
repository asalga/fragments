precision mediump float;

uniform sampler2D t0;
uniform vec2 res;

float sdCircle(vec2 p, float r){
	return length(p)-r/2.;
}

void main() {
	vec2 as = vec2(1., res.y/res.x);
	vec2 p = as * ((gl_FragCoord.xy /res) *2. -1.);
  vec3 col = texture2D(t0, (p+1.)/2. ).rgb;

  float c = .5;
  vec2 np = vec2(mod(p, c)) - c*0.5;
  float d = step(sdCircle(np, .25), 0.);
  col *= vec3(d);// + vec3(0,0,1);
  
	gl_FragColor = vec4(col, 1.);
}