precision mediump float;
#define PI 3.141592658
uniform vec2 u_res;

float r(in vec2 p, vec2 dim){
	vec2 uv = gl_FragCoord.xy / u_res - p;
	vec2 size = vec2(1.0 - dim)/2.0;
	vec2 bl = vec2(step(size, uv));
	vec2 tr = vec2(step(size, 1.0 - uv));
	return bl.x * bl.y * tr.x * tr.y;
}

void main(){
  vec2 uv = gl_FragCoord.xy / u_res * 2.0 - 1.;
  vec2 a = vec2(u_res.x/u_res.y , 1.0);
  uv = uv * a;
  float c = r(vec2(.0), vec2(0.25, 0.5));
  float len = length(uv);
  float r = mod(1.0 / len, 1.);
  float theta = mod(atan(uv.y / uv.x) / PI * 2., 1. );
  vec2 polarCoords = vec2(r, theta);
  gl_FragColor = vec4(vec3(polarCoords, c), 1.);
}