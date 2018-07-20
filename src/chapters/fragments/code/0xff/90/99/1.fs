precision mediump float;
uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

float aspect = u_res.x/u_res.y;

vec4 sample(vec2 offset){
  vec2 p = vec2(gl_FragCoord.xy + offset) / u_res;
  p.y = 1.0 - p.y;
  return texture2D(u_t0, p);
}

void main() {
	float numShades = 16.;
  vec2 p = (gl_FragCoord.xy / u_res);
  p.y = 1.0 - p.y;

  vec4 diffuse = texture2D(u_t0, p);
  float intensity = (diffuse.r + diffuse.g + diffuse.b) / 3.0;
  vec4 final = vec4(  vec3(floor(intensity * numShades)/ numShades), 1.0);
  gl_FragColor = final;
  
  
}