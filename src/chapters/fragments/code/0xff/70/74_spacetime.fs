precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdSquare(vec2 p, float halfW){
  vec2 _p = abs(p);
  return max(_p.x,_p.y)-halfW;
}

float every(float v, float i, float c){
	return mod(v, i) * step(mod(v, c), i);
}

void main(){
  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a * (gl_FragCoord.xy/u_res*2.-1.);
  float i;
  float t = u_time*1.;
  
  float tm = every(t, 1., 2.);

  vec2 pos = vec2(.5-tm,-.5);

  i = step(sdSquare(p+pos,0.25),0.);

  gl_FragColor = vec4(vec3(i),1.);
}