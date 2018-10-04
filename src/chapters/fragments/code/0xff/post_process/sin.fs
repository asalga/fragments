precision mediump float;

uniform sampler2D u_t0;
uniform vec2 u_res;
uniform float u_time;

const float PI = 3.14159;

void main() {
  vec2 p = gl_FragCoord.xy / u_res;
  p.y = 1.0 - p.y;
  float t = u_time*0.2;

  p.y += sin(p.x * 5. + t*10.)/ 4.;

  
  p.y = mod(p.y+t,1.);

   p.x += sin(p.y * 5. + t*10.)/ 4.;

  
  p.x = mod(p.x+t,1.);

float i = texture2D(u_t0, p).x;
	
	if(mod(p.y+t,1.)>.15){
  // i = 1.-i;
  // i -= .5;
  // col.x
  }



  gl_FragColor = vec4(vec3(i),1);
}
