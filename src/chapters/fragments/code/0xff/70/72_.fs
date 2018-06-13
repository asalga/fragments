precision mediump float;

uniform vec2 u_res;
uniform float u_time;

// from book of shaders
float sdRect(vec2 p, vec2 size){
  vec2 d = abs(p) - size;
  return min(max(d.x,d.y), .0) + length(max(d,.0));
}

float sdCircle(vec2 p, float r){
	return length(p)-r;
}

float circle(vec2 p, float r){
  return smoothstep(0.01, 0.001, sdCircle(p, r));
}

float rect(vec2 p, vec2 size){
  return step(sdRect(p, size), 0.);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;

  i = rect(p, vec2(.8)) - rect(p, vec2(.7));
  float pLen = length(p*10.)*5.;
  i *= step(sin(u_time* -12. + pLen), .0);

  i += circle(p, 0.6) * ;

  gl_FragColor = vec4(vec3(i),1.);
}