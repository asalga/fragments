precision mediump float;
uniform vec2 u_res;
uniform float u_time;
float ci(vec2 s, float r){
  vec2 a = vec2(u_res.x/u_res.y, 1.);
  vec2 uv = (gl_FragCoord.xy / u_res) * 2. -1.;
  return 1.0 - smoothstep(r,r + .013, distance(uv * a, s));
}

void main(){
  vec2 uv = gl_FragCoord.xy / u_res;
  vec2 off = vec2(0.2*sin(u_time*2.), 
  					-0.025*cos(u_time*4.));
  float c = ci(off, .5)
            -ci(off + vec2(-.13, .13), .45);
  gl_FragColor = vec4(vec3(c), 1.0);
}